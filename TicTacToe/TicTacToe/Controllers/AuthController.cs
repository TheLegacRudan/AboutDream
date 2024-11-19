using Microsoft.AspNetCore.Mvc;
using TicTacToe.Services;

namespace TicTacToe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(AuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest registerData)
        {
            Console.WriteLine("AuthController -> Register");

            if (!authService.CheckPassword(registerData.Password, registerData.PasswordCheck))
                return BadRequest("Password check failed!");

            var (isCreated, message, userCreated) = authService.CreateUser(registerData.Username, registerData.Password);

            if (!isCreated || userCreated == null)
                return BadRequest(message);

            var token = authService.GenerateJwtToken(userCreated.Username, userCreated.Id);
            return Ok(token);

        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginData)
        {
            Console.WriteLine("AuthController -> Login");

            var (isValid, message, user) = authService.VerifyPassword(loginData.Username, loginData.Password);
            
            if (!isValid || user == null)
                return BadRequest(message);

            var token = authService.GenerateJwtToken(user.Username, user.Id);
            return Ok(token);
        }

        public class LoginRequest
        {
            public required string Username { get; set; }
            public required string Password { get; set; }
        }

        public class RegisterRequest : LoginRequest
        {
            public required string PasswordCheck { get; set; }
        }
    }
}
