using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Models;
using TicTacToe.Services;
using static TicTacToe.Services.OverviewService;

namespace TicTacToe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OverviewController (OverviewService overviewService, AuthService authService) : ControllerBase
    {
        [HttpGet("profile")]
        public IActionResult ProfileOverview()
        {
            Console.WriteLine("OverviewController -> ProfileOverview");

            var (isValid, userId, _, message) = authService.DataFromToken(User);

            if (!isValid || userId == null)
                return Unauthorized(message);

            //Get profile using the extracted player id
            UserProfile? profile = overviewService.GetUserProfile(userId.Value);

            if (profile == null) return NotFound("User profile not found!");

            return Ok(profile);
        }

        [HttpGet("games")]
        public IActionResult GamesOverview(
            [FromQuery] DateTime? startingTime = null,
            [FromQuery] DateTime? endingTime = null,
            [FromQuery] GameStatus? gameStatus = null,
            [FromQuery] GameResult? gameResult = null,
            [FromQuery] string? winner = null)
        {
            Console.WriteLine("OverviewController -> GamesOverview");

            var (isValid, userId, _, message) = authService.DataFromToken(User);

            if (!isValid || userId == null)
                return Unauthorized(message);

            var filter = new GamesOverviewFilter
            {
                PlayerId = userId.Value,
                StartingTime = startingTime,
                EndingTime = endingTime,
                GameStatus = gameStatus,
                GameResult = gameResult,
                Winner = winner
            };

            //Get games for the player with the applied filters
            var games = overviewService.GetGamesOverview(filter);

            if (games == null || !games.Any())
                return NotFound("No games found for the specified criteria!");

            return Ok(games);
        }

    }
}
