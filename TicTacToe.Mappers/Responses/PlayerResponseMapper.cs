using TicTacToe.Data.DbModels;
using TicTacToe.Mappers.Responses.Interfaces;
using TicTacToe.Models.Responses;

namespace TicTacToe.Mappers.Responses
{
  public class PlayerResponseMapper : IPlayerResponseMapper
  {
    public PlayerResponse Map(DbPlayer player)
    {
      if (player is null)
      {
        return null;
      }

      return new PlayerResponse
      {
        Id = player.Id,
        Login = player.Login,
        Points = player.Points,
      };
    }
  }
}
