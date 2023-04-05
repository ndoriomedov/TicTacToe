using Microsoft.AspNetCore.Http;

namespace TicTacToe.Business.Extensions
{
  public static class HttpContextExtensions
  {
    public static Guid GetPlayerId(this HttpContext context)
    {
      if (!context.Items.ContainsKey("PlayerId"))
      {
        throw new ArgumentNullException("Value", "HttpContext does not contain PlayerId.");
      }

      string text = context.Items["PlayerId"]?.ToString();
      if (string.IsNullOrEmpty(text))
      {
        throw new ArgumentException("PlayerId value in HttpContext is empty.");
      }

      if (!Guid.TryParse(text, out Guid playerId))
      {
        throw new InvalidCastException("PlayerId '" + text + "' value in HttpContext is not in Guid format.");
      }

      return playerId;
    }
  }
}
