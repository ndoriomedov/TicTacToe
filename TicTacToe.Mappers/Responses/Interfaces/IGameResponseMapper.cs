using TicTacToe.Data.DbModels;
using TicTacToe.Models.Responses;

namespace TicTacToe.Mappers.Responses.Interfaces
{
  public interface IGameResponseMapper
  {
    GameResponse Map(DbGame game);
  }
}
