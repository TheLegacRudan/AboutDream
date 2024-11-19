using TicTacToe.Models;
using Microsoft.AspNetCore.Identity;
using TicTacToe.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TicTacToe.Services
{
    public class AuthService(AppDbContext context, IPasswordHasher<User> passwordHasher)
    {
        public bool CheckPassword(string password, string passwordCheck)
        {
            return password == passwordCheck;
        }

        public (bool isCreated, string message, User? user) CreateUser(string username, string password)
        {
            Console.WriteLine("AuthService -> CreateUser");

            if (context.Users.Any(u => u.Username == username))
                return (false, "A user with this username already exists!", null);

            User user = new()
            {
                Username = username,
                PasswordHash = HashPassword(new User { Username = username }, password)
            };

            context.Users.Add(user);
            context.SaveChanges();

            context.UserProfiles.Add(new UserProfile { PlayerId = user.Id, PlayerUsername = user.Username });
            context.SaveChanges();

            return (true, "User created successfully.", user);
        }



        public string HashPassword(User user, string password)
        {
            return passwordHasher.HashPassword(user, password);
        }

        public (bool isValid, string message, User? user) VerifyPassword(string username, string enteredPassword)
        {
            Console.WriteLine("AuthService -> VerifyPassword");

            var user = context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null || string.IsNullOrEmpty(user.PasswordHash))
                return (false, "Wrong username or password!", null); //Message could be "User not found!"

            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, enteredPassword);
            if (result == PasswordVerificationResult.Success) return (true, "Ok", user);
            else return (false, "Wrong username or password!", null); //Message could be "Password verification failed!" I chose first one because of safety
        }

        public string GenerateJwtToken(string username, int userId)
        {
            Console.WriteLine("AuthService -> GenerateJwtToken");

            var claims = new[]
            {
                new Claim("Name", username),
                new Claim("UserId", userId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("i1JcscCST3Rn1IFyOIGZiFZCckJKHGJ1"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "TicTacToeApp",
                audience: "TicTacToeApp",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public (bool IsValid, int? UserId, string? Username, string? Message) DataFromToken(ClaimsPrincipal user)
        {
            Console.WriteLine("AuthService -> DataFromToken");

            var userIdClaim = user.FindFirst("UserId");
            var usernameClaim = user.FindFirst("Name");

            if (userIdClaim == null)
                return (false, null, null, "User ID not found in token.");

            if (usernameClaim == null)
                return (false, null, null, "User username not found in token.");

            if (!int.TryParse(userIdClaim.Value, out int userId))
                return (false, null, null, "Invalid user ID in token.");

            string username = usernameClaim.Value;

            return (true, userId, username, null);
        }
    }
}
