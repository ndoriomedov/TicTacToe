using TModels = TicTacToe.Models.Models;

namespace TicTacToe.Business.Commands.Game.Interfaces
{
  public interface ISaveGameCommand
  {
    Task ExecuteAsync(TModels.Game game);
  }
}
