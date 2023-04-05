using TicTacToe.Business.Commands.Game.Interfaces;
using TicTacToe.Data.DbModels;
using TicTacToe.Data.Repositories.Interfaces;
using TicTacToe.Mappers.Responses.Interfaces;
using TicTacToe.Models.Requests;
using TicTacToe.Models.Responses;

namespace TicTacToe.Business.Commands.Game
{
  public class FindGamesCommand : IFindGamesCommand
  {
    private readonly IGameRepository _gameRepository;
    private readonly IGameResponseMapper _gameResponseMapper;

    public FindGamesCommand(
      IGameRepository gameRepository,
      IGameResponseMapper gameResponseMapper)
    {
      _gameRepository = gameRepository;
      _gameResponseMapper = gameResponseMapper;
    }

    public async Task<List<GameResponse>> ExecuteAsync(FindGamesFilter filter)
    {
      List<DbGame> dbGames = await _gameRepository.FindAsync(filter);

      return dbGames.ConvertAll(_gameResponseMapper.Map);
    }
  }
}
