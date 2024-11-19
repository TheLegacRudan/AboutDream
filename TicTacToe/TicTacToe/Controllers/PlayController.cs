using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Services;

namespace TicTacToe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlayController(GameManagerService gameManager, RunningGameService runningGameService, OverviewService overviewService, AuthService authService) : ControllerBase
    {
        [HttpPost("move")]
        public IActionResult MakeMove([FromBody] MoveRequest move)
        {
            Console.WriteLine("PlayController -> MakeMove");

            var (isValid, userId, username, message) = authService.DataFromToken(User);

            if (!isValid || userId == null || username == null)
                return Unauthorized(message);

            var game = gameManager.GetGame(userId.Value);

            if (game == null)
                return BadRequest("Game not found or game over!");

            var (isValidScnd, messageScnd) = runningGameService.MakeMove(game, move.Row, move.Col, userId.Value, username);
            
            if (!isValidScnd)
                return BadRequest(messageScnd);

            if (game.GameOver)
            {
                overviewService.UpdateGameOverview(game);
                overviewService.UpdateUserProfiles(game);
            }

            return Ok(new
            {
                BoardVisualized = runningGameService.VisualizeBoard(game),
                game.Board,
                game.CurrentPlayer,
                Result = game.Result.ToString(),
                game.GameOver,
                game.Winner,
                game.WinnerId,
                game.PlayerUsernames
            });
        }

        [HttpGet("status")]
        public IActionResult Status()
        {
            Console.WriteLine("PlayController -> Status");

            var (isValid, userId, _, message) = authService.DataFromToken(User);

            if (!isValid || userId == null)
                return Unauthorized(message);

            var game = gameManager.GetGame(userId.Value);

            if (game == null)
                return BadRequest("Game not found or game over!");

            return Ok(new
            {
                BoardVisualized = runningGameService.VisualizeBoard(game),
                game.Board,
                game.CurrentPlayer,
                Result = game.Result.ToString(),
                game.GameOver,
                game.Winner,
                game.WinnerId,
                game.PlayerUsernames
            });
        }

        public class MoveRequest
        {
            public int Row { get; set; }
            public int Col { get; set; }
        }
    }

}
