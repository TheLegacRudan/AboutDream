using TicTacToe.Models;

namespace TicTacToe.Services
{
    public class GameManagerService
    {
        private readonly Dictionary<int, RunningGame> _runningGames = new();

        public RunningGame StartGame(int gameId, int[] players, string[] usernames)
        {
            Console.WriteLine("GameManagerService -> StartGame");

            if (_runningGames.ContainsKey(gameId))
                throw new InvalidOperationException("Game is already running.");

            var runningGame = new RunningGame
            {
                GameId = gameId,
                Players = players,
                PlayerUsernames = usernames
            };

            _runningGames[gameId] = runningGame;

            return runningGame;
        }

        public RunningGame? GetGame(int playerId)
        {
            Console.WriteLine("GameManagerService -> GetGame");

            foreach (var runningGame in _runningGames.Values)
            {
                if (runningGame.Players.Contains(playerId) && runningGame.GameOver == false)
                    return runningGame;
            }

            Console.WriteLine($"No active games for PlayerId={playerId}");
            return null;
        }

    }

}
