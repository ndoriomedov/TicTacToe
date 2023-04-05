using TicTacToe.Business.Commands.Player.Interfaces;
using TicTacToe.Data.DbModels;
using TicTacToe.Data.Repositories.Interfaces;
using TicTacToe.Mappers.DbModels.Interfaces;
using TicTacToe.Models.Requests;

namespace TicTacToe.Business.Commands.Player
{
  public class CreatePlayerCommand : ICreatePlayerCommand
  {
    private readonly IPlayerRepository _playerRepository;
    private readonly IDbPlayerMapper _dbPlayerMapper;

    public CreatePlayerCommand(
      IPlayerRepository playerRepository,
      IDbPlayerMapper dbPlayerMapper)
    {
      _playerRepository = playerRepository;
      _dbPlayerMapper = dbPlayerMapper;
    }

    public async Task<Guid?> ExecuteAsync(CreatePlayerRequest request)
    {
      DbPlayer newPlayer = _dbPlayerMapper.Map(request);

      if (newPlayer is null)
      {
        return null;
      }

      await _playerRepository.CreateAsync(newPlayer);

      return newPlayer.Id;
    }
  }
}
