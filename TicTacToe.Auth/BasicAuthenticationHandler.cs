using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using TicTacToe.Auth.CredentialsValidators;

namespace TicTacToe.Auth
{
  public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
  {
    private readonly ICredentialsValidator _credentialsValidator;

    public BasicAuthenticationHandler(
      IOptionsMonitor<AuthenticationSchemeOptions> options,
      ILoggerFactory logger,
      UrlEncoder encoder,
      ISystemClock clock,
      ICredentialsValidator credentialsValidator)
      : base(options, logger, encoder, clock)
    {
      _credentialsValidator = credentialsValidator;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
      if (!Request.Headers.ContainsKey("Authorization"))
      {
        return AuthenticateResult.Fail("Missing Authorization Header");
      }

      AuthenticationHeaderValue authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

      if (authHeader.Scheme != "Basic")
      {
        return AuthenticateResult.Fail("Invalid Authorization Scheme");
      }

      byte[] credentialsBytes = Convert.FromBase64String(authHeader.Parameter);
      string[] credentials = Encoding.UTF8.GetString(credentialsBytes).Split(new[] { ':' }, 2);
      string username = credentials[0];
      string password = credentials[1];

      if (!await _credentialsValidator.ValidateCredentials(username, password))
      {
        return AuthenticateResult.Fail("Invalid Username or Password");
      }

      Claim[] claims = new[]
      {
        new Claim(ClaimTypes.NameIdentifier, username),
        new Claim(ClaimTypes.Name, username)
      };

      ClaimsIdentity identity = new(claims, Scheme.Name);
      ClaimsPrincipal principal = new(identity);
      AuthenticationTicket ticket = new(principal, Scheme.Name);

      return AuthenticateResult.Success(ticket);
    }
  }
}
