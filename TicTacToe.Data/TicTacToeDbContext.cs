using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TicTacToe.Data.DbModels;

namespace TicTacToe.Data
{
  public class TicTacToeDbContext : DbContext
  {
    public DbSet<DbGame> Games { get; set; }
    public DbSet<DbMove> Moves { get; set; }
    public DbSet<DbPlayer> Players { get; set; }

    public TicTacToeDbContext(DbContextOptions<TicTacToeDbContext> options)
      : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
  }
}
