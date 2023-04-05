using Microsoft.EntityFrameworkCore;
using TicTacToe.Data.DbModels;
using TicTacToe.Data.Repositories.Interfaces;

namespace TicTacToe.Data.Repositories
{
  public class PlayerRepository : IPlayerRepository
  {
    private readonly TicTacToeDbContext _dbContext;

    public PlayerRepository(TicTacToeDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public Task CreateAsync(DbPlayer player)
    {
      if (player is null)
      {
        return Task.CompletedTask;
      }

      _dbContext.Players.Add(player);

      return _dbContext.SaveChangesAsync();
    }

    public Task<List<DbPlayer>> GetAsync(List<Guid> playersIds)
    {
      return _dbContext.Players.Where(p => playersIds.Contains(p.Id)).ToListAsync();
    }

    public Task<DbPlayer> GetAsync(string login)
    {
      return _dbContext.Players.FirstOrDefaultAsync(p => p.Login.ToLower() == login.ToLower());
    }

    public Task<bool> DoesExistAsync(string login)
    {
      return _dbContext.Players.AnyAsync(p => p.Login.ToLower() == login.ToLower());
    }
  }
}
