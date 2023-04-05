namespace TicTacToe.Models.Responses
{
  public class GameResponse
  {
    public Guid Id { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid? WinnerId { get; set; }

    public List<MoveResponse> Moves { get; set; }
    public List<PlayerResponse> Players { get; set; }
  }
}
