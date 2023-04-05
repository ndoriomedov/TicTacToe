using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TicTacToe.Models.Requests
{
  public class FindGamesFilter
  {
    [FromQuery(Name = "SkipCount")]
    [Range(0, int.MaxValue)]
    public int SkipCount { get; set; } = 0;

    [FromQuery(Name = "TakeCount")]
    [Range(0, int.MaxValue)]
    public int TakeCount { get; set; } = 1;

    [FromQuery(Name = "PlayerId")]
    public Guid? PlayerId { get; set; }
  }
}
