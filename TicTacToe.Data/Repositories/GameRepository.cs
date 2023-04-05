using Microsoft.EntityFrameworkCore;
using TicTacToe.Data.DbModels;
using TicTacToe.Data.Repositories.Interfaces;
using TicTacToe.Models.Requests;

namespace TicTacToe.Data.Repositories
{
  public class GameRepository : IGameRepository
  {
    private readonly TicTacToeDbContext _dbContext;

    public GameRepository(TicTacToeDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public Task AddGameAsync(DbGame game)
    {
      _dbContext.Games.Add(game);

      return _dbContext.SaveChangesAsync();
    }

    public Task<List<DbGame>> FindAsync(FindGamesFilter filter)
    {
      if (filter is null)
      {
        return null;
      }

      IQueryable<DbGame> gamesQuery = _dbContext.Games
        .Include(g => g.Players)
        .Include(g => g.Moves)
        .AsNoTracking();

      if (filter.PlayerId.HasValue)
      {
        gamesQuery = gamesQuery.Where(g => g.Players.Any(p => p.Id == filter.PlayerId.Value));
      }

      return gamesQuery.Skip(filter.SkipCount).Take(filter.TakeCount).ToListAsync();
    }
  }
}
