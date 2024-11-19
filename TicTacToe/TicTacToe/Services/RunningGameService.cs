using TicTacToe.Models;

namespace TicTacToe.Services
{
    public class RunningGameService
    {
        public (bool isValid, string message) MakeMove(RunningGame game, int row, int col, int playerId, string username)
        {
            Console.WriteLine("RunningGameService -> MakeMove");
            
            if (game.GameOver)
                return (false, "Game finished!");

            //Validates move
            if (row < 0 || row >= 3 || col < 0 || col >= 3 || game.Board[row][col] != '-')
                return (false, "Move not valid!");

            //Maps current player symbol to ID
            var currentPlayerId = game.CurrentPlayer == 'X' ? game.Players[0] : game.Players[1];
            
            if (currentPlayerId != playerId)
                return (false, "Opponents turn!");

            game.Board[row][col] = game.CurrentPlayer;

            Console.WriteLine("Updated board:");
            foreach (var line in VisualizeBoard(game)) Console.WriteLine(line);

            //Checks for a win or draw
            if (CheckWin(game, game.CurrentPlayer))
            {
                game.GameOver = true;
                game.Result = game.CurrentPlayer == 'X' ? GameResult.WinX : GameResult.WinY;
                game.Winner = username;
                game.WinnerId = currentPlayerId;

            }
            else if (IsBoardFull(game))
            {
                game.GameOver = true;
                game.Result = GameResult.Draw;
            }
            else
            {
                //Switch the current player
                game.CurrentPlayer = game.CurrentPlayer == 'X' ? 'O' : 'X';
            }

            return (true, "Ok");
        }


        private bool CheckWin(RunningGame game, char player)
        {
            Console.WriteLine("RunningGameService -> CheckWin");

            //Checks rows
            for (int i = 0; i < 3; i++)
            {
                if ((game.Board[i][0] == player && game.Board[i][1] == player && game.Board[i][2] == player) ||
                    (game.Board[0][i] == player && game.Board[1][i] == player && game.Board[2][i] == player))
                {
                    return true;
                }
            }

            //Checks diagonals
            return (game.Board[0][0] == player && game.Board[1][1] == player && game.Board[2][2] == player) ||
                   (game.Board[0][2] == player && game.Board[1][1] == player && game.Board[2][0] == player);
        }

        private bool IsBoardFull(RunningGame game)
        {
            Console.WriteLine("RunningGameService -> IsBoardFull");

            foreach (var row in game.Board)
            {
                if (row.Contains('-'))
                    return false;
            }
            return true;
        }

        public List<string> VisualizeBoard(RunningGame game)
        {
            Console.WriteLine("RunningGameService -> VisualizeBoard");

            var visualizedBoard = new List<string>();
            for (int i = 0; i < game.Board.Count; i++)
            {
                var line = string.Join(" | ", game.Board[i]);
                visualizedBoard.Add(line);
                if (i < game.Board.Count - 1) visualizedBoard.Add("--+---+--");
            }
            return visualizedBoard;
        }
    }
}
