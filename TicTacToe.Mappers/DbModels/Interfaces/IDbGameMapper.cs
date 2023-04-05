using TicTacToe.Data.DbModels;
using TicTacToe.Models.Models;

namespace TicTacToe.Mappers.DbModels.Interfaces
{
  public interface IDbGameMapper
  {
    DbGame Map(Game game);
  }
}
