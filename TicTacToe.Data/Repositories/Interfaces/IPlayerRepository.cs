using TicTacToe.Data.DbModels;

namespace TicTacToe.Data.Repositories.Interfaces
{
  public interface IPlayerRepository
  {
    Task CreateAsync(DbPlayer player);
    Task<List<DbPlayer>> GetAsync(List<Guid> playersIds);
    Task<DbPlayer> GetAsync(string login);
    Task<bool> DoesExistAsync(string login);
  }
}
