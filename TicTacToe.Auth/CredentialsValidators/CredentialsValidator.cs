using Microsoft.AspNetCore.Http;
using TicTacToe.Auth.PasswordHasher;
using TicTacToe.Data.DbModels;
using TicTacToe.Data.Repositories.Interfaces;

namespace TicTacToe.Auth.CredentialsValidators
{
  public class CredentialsValidator : ICredentialsValidator
  {
    private readonly IPlayerRepository _playerRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CredentialsValidator(
      IPlayerRepository playerRepository,
      IPasswordHasher passwordHasher,
      IHttpContextAccessor httpContextAccessor)
    {
      _playerRepository = playerRepository;
      _passwordHasher = passwordHasher;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> ValidateCredentials(string login, string password)
    {
      DbPlayer player = await _playerRepository.GetAsync(login);

      if (player is null)
      {
        return false;
      }

      bool validatonResult = player.PasswordHash == _passwordHasher.HashPassword(login, password, player.Id.ToString());

      if (validatonResult)
      {
        _httpContextAccessor.HttpContext.Items.Add("PlayerId", player.Id);
      }

      return validatonResult;
    }
  }
}
