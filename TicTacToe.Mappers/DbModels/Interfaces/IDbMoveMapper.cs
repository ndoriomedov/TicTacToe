using TicTacToe.Data.DbModels;
using TicTacToe.Models.Models;

namespace TicTacToe.Mappers.DbModels.Interfaces
{
  public interface IDbMoveMapper
  {
    DbMove Map(Move move, Guid gameId);
  }
}
