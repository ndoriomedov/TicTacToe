namespace TicTacToe.Models.Responses
{
  public class MoveResponse
  {
    public Guid PlayerId { get; set; }
    public Guid GameId { get; set; }
    public byte Row { get; set; }
    public byte Column { get; set; }
    public DateTime CreatedAtUtc { get; set; }
  }
}
