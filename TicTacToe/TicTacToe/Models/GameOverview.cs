using System.ComponentModel.DataAnnotations;

namespace TicTacToe.Models
{
    public class GameOverview
    {
        [Key]
        public int GameId { get; set; } // Primary key
        [Required]
        public int[] Players { get; set; } = [];
        public string[] PlayerUsernames { get; set; } = [];
        public DateTime Date { get; set; } = DateTime.Now;
        public GameStatus Status { get; set; } = GameStatus.InProgress;
        public GameResult Result { get; set; } = GameResult.InProgress;
        public string? Winner { get; set; } = null;
        public int? WinnerId { get; set; } = null;
        public GameOverview() { }
        public GameOverview(int[] playersId, string[] playerUsernames)
        {
            Players = playersId;
            PlayerUsernames = playerUsernames;
        }
    }
}
