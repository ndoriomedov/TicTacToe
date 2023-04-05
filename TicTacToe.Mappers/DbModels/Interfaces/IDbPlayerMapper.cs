using TicTacToe.Data.DbModels;
using TicTacToe.Models.Requests;

namespace TicTacToe.Mappers.DbModels.Interfaces
{
  public interface IDbPlayerMapper
  {
    DbPlayer Map(CreatePlayerRequest request);
  }
}
