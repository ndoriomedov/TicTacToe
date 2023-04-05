namespace TicTacToe.Auth.CredentialsValidators
{
  public interface ICredentialsValidator
  {
    Task<bool> ValidateCredentials(string login, string password);
  }
}
