using System.ComponentModel.DataAnnotations;

namespace TicTacToe.Models
{
    public class UserProfile
    {
        [Key]
        public int ProfileId { get; set; } // Primary key
        [Required]
        public int PlayerId { get; set; }
        public string? PlayerUsername { get; set; }
        public int GamesPlayed { get; set; } = 0;
        public int GamesWon { get; set; } = 0;
        public int WinPercentage { get; set; } = 0;
        public UserProfile() { }
        public UserProfile(int id, string username)
        {
            PlayerId = id;
            PlayerUsername = username;
        }
    }
}
