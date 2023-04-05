using TicTacToe.Data.DbModels;
using TicTacToe.Models.Requests;

namespace TicTacToe.Data.Repositories.Interfaces
{
  public interface IGameRepository
  {
    Task AddGameAsync(DbGame game);
    Task<List<DbGame>> FindAsync(FindGamesFilter filter);
  }
}
