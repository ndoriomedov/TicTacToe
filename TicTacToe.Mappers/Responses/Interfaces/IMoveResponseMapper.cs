using TicTacToe.Data.DbModels;
using TicTacToe.Models.Responses;

namespace TicTacToe.Mappers.Responses.Interfaces
{
  public interface IMoveResponseMapper
  {
    MoveResponse Map(DbMove move);
  }
}
