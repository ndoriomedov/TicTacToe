namespace TicTacToe.Auth.PasswordHasher
{
  public interface IPasswordHasher
  {
    string HashPassword(string login, string password, string salt);
  }
}
