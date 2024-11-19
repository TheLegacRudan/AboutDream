using System.ComponentModel.DataAnnotations;

namespace TicTacToe.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; } // Primary key
        [Required]
        [MaxLength(50)]
        public required string Username { get; set; }
        public string? PasswordHash { get; set; }
        public User() { }
        public User(string username)
        {
            Username = username;
        }
    }
}
