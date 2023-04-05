using TicTacToe.Data.DbModels;
using TicTacToe.Mappers.DbModels.Interfaces;
using TicTacToe.Models.Models;

namespace TicTacToe.Mappers.DbModels
{
  public class DbMoveMapper : IDbMoveMapper
  {
    public DbMove Map(Move move, Guid gameId)
    {
      if (move is null)
      {
        return null;
      }

      return new DbMove
      {
        Id = Guid.NewGuid(),
        PlayerId = move.PlayerId,
        GameId = gameId,
        Row = move.Row,
        Column = move.Column,
        CreatedAtUtc = move.CreatedAtUtc,
      };
    }
  }
}
