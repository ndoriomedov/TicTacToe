namespace TicTacToe.Models.Models
{
  public class Move
  {
    public Guid PlayerId { get; set; }
    public byte Row { get; set; }
    public byte Column { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public Move(Guid playerId, byte row, byte column, DateTime createdAtUtc)
    {
      PlayerId = playerId;
      Row = row;
      Column = column;
      CreatedAtUtc = createdAtUtc;
    }
  }
}
