using TicTacToe.Data.DbModels;
using TicTacToe.Models.Responses;

namespace TicTacToe.Mappers.Responses.Interfaces
{
  public interface IPlayerResponseMapper
  {
    PlayerResponse Map(DbPlayer player);
  }
}
