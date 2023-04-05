using TicTacToe.Models.Requests;

namespace TicTacToe.Business.Commands.Player.Interfaces
{
  public interface ICreatePlayerCommand
  {
    Task<Guid?> ExecuteAsync(CreatePlayerRequest request);
  }
}
