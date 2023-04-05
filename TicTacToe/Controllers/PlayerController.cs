using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Business.Commands.Player.Interfaces;
using TicTacToe.Models.Requests;
using TicTacToe.Validation.Player.Interfaces;

namespace TicTacToe.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class PlayerController : ControllerBase
  {
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<Guid?>> CreatePlayerAsync(
      [FromBody] CreatePlayerRequest request,
      [FromServices] ICreatePlayerCommand command,
      [FromServices] ICreatePlayerRequestValidator requestValidator)
    {
      ValidationResult validationResult = await requestValidator.ValidateAsync(request);

      return validationResult.IsValid
        ? Ok(await command.ExecuteAsync(request))
        : BadRequest(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
    }
  }
}
