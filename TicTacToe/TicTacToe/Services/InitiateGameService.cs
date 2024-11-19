using TicTacToe.Data;
using TicTacToe.Models;

namespace TicTacToe.Services
{
    public class InitiateGameService(AppDbContext context, GameManagerService gameManager)
    {
        public (bool IsValid, string Message, GameOverview? Game) CreateGame(int playerId, string username)
        {
            Console.WriteLine("InitiateGameService -> CreateGame");

            //Checks if player has an active game
            if (HasActiveGame(playerId))
                return (false, "Player already has an active game.", null);

            var game = new GameOverview
            {
                Players = [playerId],
                PlayerUsernames = [username],
                Status = GameStatus.Waiting,
                Date = DateTime.UtcNow
            };

            context.GameOverviews.Add(game);
            context.SaveChanges();

            return (true, "Game created successfully.", game);
        }

        public (bool IsValid, string Message, GameOverview? Game) JoinGame(int playerId, string username)
        {
            Console.WriteLine("InitiateGameService -> JoinGame");

            if (HasActiveGame(playerId))
                return (false, "Player already has an active game.", null);

            //Finds a game to join
            var game = context.GameOverviews
                .FirstOrDefault(go => go.Status == GameStatus.Waiting && !go.Players.Contains(playerId));

            if (game == null)
                return (false, "No available games to join.", null);

            game.Players = [.. game.Players, playerId];
            game.PlayerUsernames = [.. game.PlayerUsernames, username];

            if (game.Players.Length == 2)
            {
                game.Status = GameStatus.InProgress;
                //Create a RunningGame instance
                gameManager.StartGame(game.GameId, game.Players, game.PlayerUsernames);
            }

            context.SaveChanges();
            return (true, "Joined game successfully.", game);
        }

        private bool HasActiveGame(int playerId)
        {
            Console.WriteLine("InitiateGameService -> HasActiveGame");

            return context.GameOverviews
                .Any(go => go.Players.Contains(playerId) &&
                          (go.Status == GameStatus.Waiting || go.Status == GameStatus.InProgress));
        }
    }


}
