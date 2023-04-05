using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using TicTacToe.Business.Commands.Game.Interfaces;
using TicTacToe.Business.Extensions;
using TicTacToe.Models.Configurations;
using TicTacToe.Models.Models;

namespace TicTacToe.Hubs
{
  public class TicTacToeHub : Hub
  {
    private static ConcurrentQueue<Game> _waitingGames = new();

    private static ConcurrentDictionary<string, Guid> _connectionsPlayers = new();
    private static ConcurrentDictionary<string, Game> _connectionsGames = new();
    private static ConcurrentDictionary<Guid, Game> _playersGames = new();

    private readonly ISaveGameCommand _saveGameCommand;
    private readonly SignalREndpointsConfiguration _endpoints;

    public TicTacToeHub(
      ISaveGameCommand saveGameCommand,
      IOptions<SignalREndpointsConfiguration> endpointsConfiguration)
    {
      _saveGameCommand = saveGameCommand;
      _endpoints = endpointsConfiguration.Value;
    }

    public override Task OnConnectedAsync()
    {
      Guid playerId = Context.GetHttpContext().GetPlayerId();

      _connectionsPlayers.TryAdd(Context.ConnectionId, playerId);

      return base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
      if (_connectionsGames.TryGetValue(Context.ConnectionId, out Game abortedGame))
      {
        await EndAbortedGame(abortedGame);
      }
      else
      {
        _connectionsPlayers.Remove(Context.ConnectionId, out Guid _);
      }

      await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinGame()
    {
      if (!_connectionsPlayers.TryGetValue(Context.ConnectionId, out Guid playerId))
      {
        return;
      }

      if (_playersGames.TryGetValue(playerId, out Game _))
      {
        return;
      }

      if (_waitingGames.TryDequeue(out Game game))
      {
        game.PlayerO = (Context.ConnectionId, playerId);

        if (!TryAddUserData(game, playerId))
        {
          // return game to waiting for second player list
          _waitingGames.Enqueue(game);

          return;
        }

        try
        {
          await Groups.AddToGroupAsync(Context.ConnectionId, game.Id.ToString());

          await Clients.Group(game.Id.ToString()).SendAsync(_endpoints.GameStartedEndpoint);
        }
        catch
        {
          await Groups.RemoveFromGroupAsync(Context.ConnectionId, game.Id.ToString());

          _waitingGames.Enqueue(game);

          throw;
        }

        game.UpdateTimer(DateTime.UtcNow);
      }
      else
      {
        game = new Game()
        {
          PlayerX = (Context.ConnectionId, playerId),
        };

        if (!TryAddUserData(game, playerId))
        {
          return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, game.Id.ToString());

        await Clients.Group(game.Id.ToString()).SendAsync(_endpoints.WaitingForOpponentEndpoint);

        _waitingGames.Enqueue(game);
      }
    }

    public async Task MakeMove(byte row, byte column)
    {
      DateTime requestRecieveTime = DateTime.UtcNow;

      if (!_connectionsGames.TryGetValue(Context.ConnectionId, out Game currentGame))
      {
        return;
      }

      (bool isValidMove, bool isGameEnded) moveResult = currentGame.MakeMove(Context.ConnectionId, row, column, requestRecieveTime);

      if (moveResult.isGameEnded)
      {
        Task sendingEndMessageTask = currentGame.WinnerId is null
          ? Clients.Group(currentGame.Id.ToString()).SendAsync(_endpoints.GameEndedEndpoint)
          : Clients.Group(currentGame.Id.ToString()).SendAsync(_endpoints.GameEndedEndpoint, currentGame.GetWinnerSymbol());

        ResetGamePlayers(currentGame);

        try
        {
          await sendingEndMessageTask;
        }
        catch
        {
          throw;
        }
        finally
        {
          await _saveGameCommand.ExecuteAsync(currentGame);
        }

        return;
      }

      if (!moveResult.isValidMove)
      {
        await Clients.Caller.SendAsync(_endpoints.InvalidMoveEndpoint);

        return;
      }

      await Clients.Group(currentGame.Id.ToString()).SendAsync(_endpoints.MoveMadeEndpoint, row, column, currentGame.GetCurrentSymbol());

      currentGame.UpdateTimer(DateTime.UtcNow);
    }

    // calls when move timer on frontend is expired
    public async Task TimerExpired()
    {
      if (!_connectionsGames.TryGetValue(Context.ConnectionId, out Game currentGame))
      {
        return;
      }

      await EndAbortedGame(currentGame);
    }

    #region private methods

    private async Task EndAbortedGame(Game abortedGame)
    {
      abortedGame.SetWinner(Context.ConnectionId);

      try
      {
        await Clients.Group(abortedGame.Id.ToString()).SendAsync(_endpoints.GameEndedEndpoint, abortedGame.GetWinnerSymbol());
      }
      catch
      {
        throw;
      }
      finally
      {
        ResetGamePlayers(abortedGame);

        await _saveGameCommand.ExecuteAsync(abortedGame);
      }
    }

    private void ResetGamePlayers(Game game)
    {
      _connectionsGames.Remove(game.PlayerO.connectionId, out Game _);
      _playersGames.Remove(game.PlayerO.playerId, out Game _);
      _connectionsPlayers.Remove(game.PlayerO.connectionId, out Guid _);

      _connectionsGames.Remove(game.PlayerX.connectionId, out Game _);
      _playersGames.Remove(game.PlayerX.playerId, out Game _);
      _connectionsPlayers.Remove(game.PlayerX.connectionId, out Guid _);
    }

    private bool TryAddUserData(Game game, Guid playerId)
    {
      if (_connectionsGames.TryAdd(Context.ConnectionId, game))
      {
        if (_playersGames.TryAdd(playerId, game))
        {
          return true;
        }
        else
        {
          _connectionsGames.Remove(Context.ConnectionId, out Game _);
        }
      }

      return false;
    }

    #endregion
  }
}
