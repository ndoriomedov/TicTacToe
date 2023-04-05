using System.Security.Cryptography;
using System.Text;

namespace TicTacToe.Auth.PasswordHasher
{
  public class PasswordHasher : IPasswordHasher
  {
    const int OutputLength = 64;
    const int Iterations = 350000;
    private readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

    public string HashPassword(string login, string password, string salt)
    {
      byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
        Encoding.UTF8.GetBytes($"{login}{password}"),
        Encoding.UTF8.GetBytes(salt),
        Iterations,
        _hashAlgorithm,
        OutputLength);

      return Convert.ToHexString(hash);
    }
  }
}
