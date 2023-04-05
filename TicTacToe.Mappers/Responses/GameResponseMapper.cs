using TicTacToe.Data.DbModels;
using TicTacToe.Mappers.Responses.Interfaces;
using TicTacToe.Models.Responses;

namespace TicTacToe.Mappers.Responses
{
  public class GameResponseMapper : IGameResponseMapper
  {
    private readonly IMoveResponseMapper _moveResponseMapper;
    private readonly IPlayerResponseMapper _playerResponseMapper;

    public GameResponseMapper(
      IMoveResponseMapper moveResponseMapper,
      IPlayerResponseMapper playerResponseMapper)
    {
      _playerResponseMapper = playerResponseMapper;
      _moveResponseMapper = moveResponseMapper;
    }

    public GameResponse Map(DbGame game)
    {
      if (game is null)
      {
        return null;
      }

      return new GameResponse
      {
        Id = game.Id,
        CreatedAtUtc = game.CreatedAtUtc,
        WinnerId = game.WinnerId,

        Moves = game.Moves?.Select(_moveResponseMapper.Map).ToList(),
        Players = game.Players?.Select(_playerResponseMapper.Map).ToList(),
      };
    }
  }
}
