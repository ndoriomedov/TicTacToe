using TicTacToe.Auth.PasswordHasher;
using TicTacToe.Data.DbModels;
using TicTacToe.Mappers.DbModels.Interfaces;
using TicTacToe.Models.Requests;

namespace TicTacToe.Mappers.DbModels
{
  public class DbPlayerMapper : IDbPlayerMapper
  {
    private readonly IPasswordHasher _passwordHasher;

    public DbPlayerMapper(
      IPasswordHasher passwordHasher)
    {
      _passwordHasher = passwordHasher;
    }

    public DbPlayer Map(CreatePlayerRequest request)
    {
      if (request is null)
      {
        return null;
      }

      Guid userId = Guid.NewGuid();

      return new DbPlayer
      {
        Id = userId,
        Login = request.Login,
        PasswordHash = _passwordHasher.HashPassword(request.Login, request.Password, userId.ToString()),
        Points = 0,
      };
    }
  }
}
