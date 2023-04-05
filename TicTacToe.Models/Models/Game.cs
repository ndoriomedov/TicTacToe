namespace TicTacToe.Models.Models
{
  public class Game
  {
    const bool X = false;
    const bool O = true;
    const double MoveTimeConstraintInSeconds = 15d;

    private bool _lastMoveFigure;
    private byte _movesCount;
    private DateTime _lastMoveDateTime;
    private bool?[,] _board;

    public Guid Id { get; set; }
    public DateTime CreatedAtUtc { get; init; }
    public (string connectionId, Guid playerId) PlayerX { get; set; }
    public (string connectionId, Guid playerId) PlayerO { get; set; }
    public Guid? WinnerId { get; set; }
    public List<Move> Moves { get; set; }
    
    public Game()
    {
      Id = Guid.NewGuid();
      _board = new bool?[3, 3];
      CreatedAtUtc = DateTime.UtcNow;

      _lastMoveFigure = O;

      Moves = new List<Move>();
    }

    #region public methods

    public (bool isValidMove, bool isGameEnded) MakeMove(string playerConnectionId, byte row, byte column, DateTime requestRecieveTime)
    {
      if (_lastMoveFigure == X && playerConnectionId == PlayerX.connectionId
        || _lastMoveFigure == O && playerConnectionId == PlayerO.connectionId)
      {
        return (isValidMove: false, isGameEnded: false);
      }

      Guid currentPlayerId = GetCurrentPlayerId(false);

      if (!CheckTimer(requestRecieveTime))
      {
        WinnerId = GetCurrentPlayerId(true);

        return (isValidMove: false, isGameEnded: true);
      }

      if (!UpdateBoard(row, column))
      {
        return (isValidMove: false, isGameEnded: false);
      }

      Moves.Add(new Move(currentPlayerId, row, column, requestRecieveTime));

      if (CheckWin())
      {
        WinnerId = currentPlayerId;

        return (isValidMove: true, isGameEnded: true);
      }

      if (CheckDraw())
      {
        return (isValidMove: true, isGameEnded: true);
      }

      return (isValidMove: true, isGameEnded: false);
    }

    public void UpdateTimer(DateTime responseSendTime)
    {
      _lastMoveDateTime = responseSendTime;
    }

    public char GetWinnerSymbol()
    {
      if (!WinnerId.HasValue)
      {
        return default;
      }

      return WinnerId.Value == PlayerX.playerId
        ? 'X'
        : 'O';
    }

    public char GetCurrentSymbol()
    {
      return _lastMoveFigure == X
        ? 'X'
        : 'O';
    }

    public void SetWinner(string loserConnectionId)
    {
      WinnerId = loserConnectionId == PlayerX.connectionId
        ? PlayerO.playerId
        : PlayerX.playerId;
    }

    #endregion

    #region private methods

    private bool CheckTimer(DateTime requestRecieveTime)
    {
      return (requestRecieveTime.ToUniversalTime() - _lastMoveDateTime).TotalSeconds > MoveTimeConstraintInSeconds;
    }

    private bool UpdateBoard(int row, int column)
    {
      if (_board[row, column] is null && row < _board.GetLength(0) && column < _board.GetLength(1))
      {
        _lastMoveFigure = !_lastMoveFigure;

        _board[row, column] = _lastMoveFigure;

        _movesCount++;

        return true;
      }

      return false;
    }

    private bool CheckWin()
    {
      if (_movesCount < 5)
      {
        return false;
      }

      return CheckRows() || CheckColumns() || CheckDiagonals();
    }

    private bool CheckDraw()
    {
      if (_movesCount < 9)
      {
        return false;
      }

      return !CheckWin();
    }

    private bool CheckRows()
    {
      for (int row = 0; row < _board.GetLength(0); row++)
      {
        if (_board[row, 0] == _lastMoveFigure && _board[row, 1] == _lastMoveFigure && _board[row, 2] == _lastMoveFigure)
        {
          return true;
        }
      }

      return false;
    }

    private bool CheckColumns()
    {
      for (int column = 0; column < _board.GetLength(0); column++)
      {
        if (_board[0, column] == _lastMoveFigure && _board[1, column] == _lastMoveFigure && _board[2, column] == _lastMoveFigure)
        {
          return true;
        }
      }

      return false;
    }

    private bool CheckDiagonals()
    {
      return (_board[0, 0] == _lastMoveFigure && _board[1, 1] == _lastMoveFigure && _board[2, 2] == _lastMoveFigure)
        || (_board[0, 2] == _lastMoveFigure && _board[1, 1] == _lastMoveFigure && _board[2, 0] == _lastMoveFigure);
    }

    private Guid GetCurrentPlayerId(bool isMoveMade)
    {
      if (isMoveMade)
      {
        return _lastMoveFigure == X
          ? PlayerX.playerId
          : PlayerO.playerId;
      }
      else
      {
        return _lastMoveFigure == X
          ? PlayerO.playerId
          : PlayerX.playerId;
      }
    }

    #endregion
  }
}
