using TicTacToe.Models.Requests;
using TicTacToe.Models.Responses;

namespace TicTacToe.Business.Commands.Game.Interfaces
{
  public interface IFindGamesCommand
  {
    Task<List<GameResponse>> ExecuteAsync(FindGamesFilter filter);
  }
}
