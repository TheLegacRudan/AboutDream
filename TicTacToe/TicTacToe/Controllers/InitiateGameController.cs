using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Services;

namespace TicTacToe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InitiateGameController(InitiateGameService initiateGameService, AuthService authService) : ControllerBase
    {
        [HttpPost("create")]
        public IActionResult CreateGame()
        {
            Console.WriteLine("InitiateGameController -> CreateGame");

            var (isValid, userId, username, message) = authService.DataFromToken(User);

            if (!isValid || userId == null || username == null)
                return Unauthorized(message);

            var (IsValidScnd, messageScnd, Game) = initiateGameService.CreateGame(userId.Value, username);

            if (!IsValidScnd || Game == null)
                return BadRequest(messageScnd);

            return Ok(new
            {
                Game.GameId,
                Game.PlayerUsernames,
                Game.Date,
                Result = Game.Result.ToString(),
                Status = Game.Status.ToString(),
                Game.Winner
            });
        }


        [HttpPost("join")]
        public IActionResult JoinGame()
        {
            Console.WriteLine("InitiateGameController -> JoinGame");

            var (isValid, userId, username, message) = authService.DataFromToken(User);

            if (!isValid || userId == null || username == null)
                return Unauthorized(message);

            var (IsValidScnd, messageScnd, Game) = initiateGameService.JoinGame(userId.Value, username);

            if (!IsValidScnd || Game == null)
                return BadRequest(messageScnd);

            return Ok(new
            {
                Game.GameId,
                Game.PlayerUsernames,
                Game.Date,
                Result = Game.Result.ToString(),
                Status = Game.Status.ToString(),
                Game.Winner
            });
        }
    }


}
