using Microsoft.EntityFrameworkCore;
using TicTacToe.Data;
using TicTacToe.Models;

namespace TicTacToe.Services
{
    public class OverviewService(AppDbContext context)
    {
        public UserProfile? GetUserProfile(int playerId)
        {
            Console.WriteLine("OverviewService -> GetUserProfile");

            UserProfile? profile = context.UserProfiles.FirstOrDefault(u => u.PlayerId == playerId);
            return profile;
        }

        public IEnumerable<GameOverview> GetGamesOverview(GamesOverviewFilter filter)
        {
            Console.WriteLine("OverviewService -> GetGamesOverview");

            var query = context.GameOverviews.AsQueryable()
                .Where(go => go.Players.Contains(filter.PlayerId));

            if (filter.GameResult != null)
            {
                query = query.Where(go => go.Result == filter.GameResult);
            }
            if (filter.GameStatus != null)
            {
                query = query.Where(go => go.Status == filter.GameStatus);
            }
            if (filter.Winner != null)
            {
                query = query.Where(go => go.Winner == filter.Winner);
            }
            if (filter.StartingTime.HasValue)
            {
                query = query.Where(go => go.Date >= filter.StartingTime.Value);
            }
            if (filter.EndingTime.HasValue)
            {
                query = query.Where(go => go.Date <= filter.EndingTime.Value);
            }

            var results = query.ToList();
            return results;
        }


        public class GamesOverviewFilter
        {
            public int PlayerId { get; set; }
            public DateTime? StartingTime { get; set; }
            public DateTime? EndingTime { get; set; }
            public GameStatus? GameStatus { get; set; }
            public GameResult? GameResult { get; set; }
            public string? Winner { get; set; }
        }


        public void UpdateGameOverview(RunningGame game)
        {
            Console.WriteLine("OverviewService -> UpdateGameOverview");
            Console.WriteLine(game.GameId.ToString());

            var gameOverview = context.GameOverviews.FirstOrDefault(go => go.GameId == game.GameId);

            if (gameOverview == null)
            {
                Console.WriteLine("GameOverview not found in the database.");
                return;
            }

            gameOverview.Status = GameStatus.Finished;
            gameOverview.Result = game.Result;
            gameOverview.Winner = !string.IsNullOrEmpty(game.Winner) ? game.Winner : "Draw";
            gameOverview.WinnerId = game.WinnerId.HasValue ? game.WinnerId : null;

            context.SaveChanges();
            Console.WriteLine("GameOverview updated successfully.");
        }

        public void UpdateUserProfiles(RunningGame game)
        {
            Console.WriteLine("OverviewService -> UpdateUserProfiles");


            foreach (var playerId in game.Players)
            {
                var userProfile = context.UserProfiles.FirstOrDefault(up => up.PlayerId == playerId);
                if (userProfile == null)
                {
                    Console.WriteLine($"UserProfile not found for PlayerId: {playerId}");
                    continue;
                }

                userProfile.GamesPlayed++;

                //If the player is winner, increases GamesWon
                if (game.WinnerId.HasValue && game.WinnerId.Value == playerId)
                {
                    userProfile.GamesWon++;
                }

                //Calculates WinPercentage
                userProfile.WinPercentage = userProfile.GamesPlayed > 0
                    ? (userProfile.GamesWon * 100) / userProfile.GamesPlayed
                    : 0;

                context.SaveChanges();
            }
        }
    }
}
