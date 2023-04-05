using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TicTacToe.Auth;
using TicTacToe.Auth.CredentialsValidators;
using TicTacToe.Auth.PasswordHasher;
using TicTacToe.Business.Commands.Game;
using TicTacToe.Business.Commands.Game.Interfaces;
using TicTacToe.Business.Commands.Player;
using TicTacToe.Business.Commands.Player.Interfaces;
using TicTacToe.Data;
using TicTacToe.Data.Repositories;
using TicTacToe.Data.Repositories.Interfaces;
using TicTacToe.Hubs;
using TicTacToe.Mappers.DbModels;
using TicTacToe.Mappers.DbModels.Interfaces;
using TicTacToe.Mappers.Responses;
using TicTacToe.Mappers.Responses.Interfaces;
using TicTacToe.Models.Configurations;
using TicTacToe.Validation.Player;
using TicTacToe.Validation.Player.Interfaces;

internal class Program
{
  private static void Main(string[] args)
  {
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    AddServices(builder.Services);

    builder.Services.AddCors(options =>
    {
      options.AddPolicy("CorsPolicy",
        builder => builder
          .WithOrigins("https://localhost:3000") // signalR origins here
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials());
    });

    builder.Services.AddAuthentication("BasicAuthentication")
      .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

    builder.Services.AddAuthorization(options =>
    {
      options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    });

    builder.Services.AddDbContext<TicTacToeDbContext>(options =>
    {
      options.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnectionString"));
    });

    builder.Services.AddControllers();
    builder.Services.AddSignalR();

    builder.Services.Configure<SignalREndpointsConfiguration>(builder.Configuration.GetSection("SignalREndpoints"));

    builder.Services.AddHttpContextAccessor();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    WebApplication app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    UpdateDatabase(app);

    app.UseCors("CorsPolicy");

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.MapHub<TicTacToeHub>("/tictactoehub");

    app.Run();
  }

  private static void AddServices(IServiceCollection services)
  {
    // commands
    services.AddTransient<ICreatePlayerCommand, CreatePlayerCommand>();
    services.AddTransient<ISaveGameCommand, SaveGameCommand>();
    services.AddTransient<IFindGamesCommand, FindGamesCommand>();

    // auth
    services.AddTransient<ICredentialsValidator, CredentialsValidator>();
    services.AddTransient<IPasswordHasher, PasswordHasher>();

    // validators
    services.AddTransient<ICreatePlayerRequestValidator, CreatePlayerRequestValidator>();

    // repos
    services.AddTransient<IGameRepository, GameRepository>();
    services.AddTransient<IPlayerRepository, PlayerRepository>();

    // mappers
    services.AddTransient<IDbGameMapper, DbGameMapper>();
    services.AddTransient<IDbMoveMapper, DbMoveMapper>();
    services.AddTransient<IDbPlayerMapper, DbPlayerMapper>();
    services.AddTransient<IPlayerResponseMapper, PlayerResponseMapper>();
    services.AddTransient<IGameResponseMapper, GameResponseMapper>();
    services.AddTransient<IMoveResponseMapper, MoveResponseMapper>();
  }

  private static void UpdateDatabase(IApplicationBuilder app)
  {
    using IServiceScope serviceScope = app.ApplicationServices
      .GetRequiredService<IServiceScopeFactory>()
      .CreateScope();

    using TicTacToeDbContext context = serviceScope.ServiceProvider.GetService<TicTacToeDbContext>();

    context.Database.Migrate();
  }
}