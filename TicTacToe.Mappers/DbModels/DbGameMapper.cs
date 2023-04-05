using TicTacToe.Data.DbModels;
using TicTacToe.Mappers.DbModels.Interfaces;
using TicTacToe.Models.Models;

namespace TicTacToe.Mappers.DbModels
{
  public class DbGameMapper : IDbGameMapper
  {
    private readonly IDbMoveMapper _dbMoveMapper;

    public DbGameMapper(
      IDbMoveMapper dbMoveMapper)
    {
      _dbMoveMapper = dbMoveMapper;
    }

    public DbGame Map(Game game)
    {
      if (game is null)
      {
        return null;
      }

      return new DbGame
      {
        Id = game.Id,
        CreatedAtUtc = game.CreatedAtUtc,
        WinnerId = game.WinnerId,

        Moves = game.Moves.ConvertAll(move => _dbMoveMapper.Map(move, game.Id)),
      };
    }
  }
}
