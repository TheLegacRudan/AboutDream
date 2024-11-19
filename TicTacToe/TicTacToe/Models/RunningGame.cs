namespace TicTacToe.Models
{
    public class RunningGame
    {
        public int GameId { get; set; }
        public List<List<char>> Board { get; set; } = new List<List<char>>
        {
            new List<char> { '-', '-', '-' },
            new List<char> { '-', '-', '-' },
            new List<char> { '-', '-', '-' }
        };

        public char CurrentPlayer { get; set; } = 'X';
        public bool GameOver { get; set; } = false;
        public GameResult Result { get; set; } = GameResult.InProgress;
        public int[] Players { get; set; } = [];
        public string[] PlayerUsernames { get; set; } = [];
        public string? Winner { get; set; } = null;
        public int? WinnerId { get; set; } = null;
    }
}
