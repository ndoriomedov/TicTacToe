using FluentValidation;
using TicTacToe.Data.Repositories.Interfaces;
using TicTacToe.Models.Requests;
using TicTacToe.Validation.Player.Interfaces;

namespace TicTacToe.Validation.Player
{
  public class CreatePlayerRequestValidator : AbstractValidator<CreatePlayerRequest>, ICreatePlayerRequestValidator
  {
    public CreatePlayerRequestValidator(
      IPlayerRepository playerRepository)
    {
      RuleFor(x => x.Login)
        .MinimumLength(1).WithMessage("Login is too short.")
        .MaximumLength(100).WithMessage("Login should not be more than 100 symbols.")
        .MustAsync(async (login, _) => !await playerRepository.DoesExistAsync(login))
        .WithMessage("Invalid login.");
    }
  }
}
