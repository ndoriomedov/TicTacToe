namespace TicTacToe.Models.Configurations
{
  public class SignalREndpointsConfiguration
  {
    public string WaitingForOpponentEndpoint { get; init; }
    public string GameStartedEndpoint { get; init; }
    public string MoveMadeEndpoint { get; init; }
    public string InvalidMoveEndpoint { get; init; }
    public string GameEndedEndpoint { get; init; }
  }
}
