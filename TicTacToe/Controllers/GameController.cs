using Microsoft.AspNetCore.Mvc;
using TicTacToe.Business.Commands.Game.Interfaces;
using TicTacToe.Models.Requests;
using TicTacToe.Models.Responses;

namespace TicTacToe.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class GameController : ControllerBase
  {
    [HttpGet]
    public async Task<ActionResult<List<GameResponse>>> GetAsync(
      [FromQuery] FindGamesFilter filter,
      [FromServices] IFindGamesCommand command)
    {
      return Ok(await command.ExecuteAsync(filter));
    }
  }
}
