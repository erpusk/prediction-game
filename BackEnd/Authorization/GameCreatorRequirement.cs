using System.Security.Claims;
using itb2203_2024_predictiongame.Backend.Data.Repos;
using Microsoft.AspNetCore.Authorization;

namespace BackEnd.Authorization
{
    public class GameCreatorRequirement : IAuthorizationRequirement { }

    public class GameCreatorHandler : AuthorizationHandler<GameCreatorRequirement> {
        private readonly PredictionGamesRepo _repo;
        public GameCreatorHandler(PredictionGamesRepo repo)
        {
            _repo = repo;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, GameCreatorRequirement requirement)
        {
            var userIdClaim = context.User.FindFirst("userId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return;
            }

            // Extract game ID from route data if using an MVC or endpoint routing setup
            var routeData = (context.Resource as HttpContext)?.GetRouteData();
            if (routeData == null || !routeData.Values.TryGetValue("predictionGameId", out var gameIdValue) ||
                !int.TryParse(gameIdValue?.ToString(), out int gameId))
            {
                return;
            }

            // Check if the user is the creator of the game
            var game = await _repo.GetPredictionGameById(gameId);
            if (game != null && game.GameCreatorId == userId)
            {
                context.Succeed(requirement);
            }

            return;
        }
    }
}