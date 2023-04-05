using TicTacToe.Data.DbModels;
using TicTacToe.Mappers.Responses.Interfaces;
using TicTacToe.Models.Responses;

namespace TicTacToe.Mappers.Responses
{
  public class MoveResponseMapper : IMoveResponseMapper
  {
    public MoveResponse Map(DbMove move)
    {
      if (move is null)
      {
        return null;
      }

      return new MoveResponse
      {
        PlayerId = move.PlayerId,
        GameId = move.GameId,
        Row = move.Row,
        Column = move.Column,
        CreatedAtUtc = move.CreatedAtUtc,
      };
    }
  }
}
