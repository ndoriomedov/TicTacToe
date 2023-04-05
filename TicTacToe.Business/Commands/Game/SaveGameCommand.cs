using TicTacToe.Business.Commands.Game.Interfaces;
using TicTacToe.Data.DbModels;
using TicTacToe.Data.Repositories.Interfaces;
using TicTacToe.Mappers.DbModels.Interfaces;

namespace TicTacToe.Business.Commands.Game
{
  public class SaveGameCommand : ISaveGameCommand
  {
    private readonly IGameRepository _gameRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IDbGameMapper _dbGameMapper;

    public SaveGameCommand(
      IGameRepository gameRepository,
      IPlayerRepository playerRepository,
      IDbGameMapper dbGameMapper)
    {
      _gameRepository = gameRepository;
      _playerRepository = playerRepository;
      _dbGameMapper = dbGameMapper;
    }

    public async Task ExecuteAsync(Models.Models.Game game)
    {
      List<DbPlayer> players = await _playerRepository.GetAsync(new List<Guid>() { game.PlayerX.playerId, game.PlayerO.playerId });

      DbGame dbGame = _dbGameMapper.Map(game);

      if (dbGame.WinnerId.HasValue)
      {
        players.First(p => p.Id == dbGame.WinnerId).Points += 3;
      }
      else
      {
        foreach (DbPlayer player in players)
        {
          player.Points += 2;
        }
      }

      dbGame.Players = players;

      await _gameRepository.AddGameAsync(dbGame);
    }
  }
}
