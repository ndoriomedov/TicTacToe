using FluentValidation;
using TicTacToe.Models.Requests;

namespace TicTacToe.Validation.Player.Interfaces
{
  public interface ICreatePlayerRequestValidator : IValidator<CreatePlayerRequest>
  {
  }
}
