using Microsoft.EntityFrameworkCore;
using itb2203_2024_predictiongame.Backend.Models.Classes;
using BackEnd.Models.Classes;
using System.Runtime.CompilerServices;
using System.IO.Compression;

namespace itb2203_2024_predictiongame.Backend.Data.Repos
{
    public class PredictionGamesRepo(DataContext context)
    {
        private readonly DataContext context = context;

        //CREATE
        public async Task<PredictionGame> CreatePredictionGameToDb(PredictionGame predictionGame)
        {
            var gameCreator = await context.ApplicationUsers.Include(u => u.CreatedPredictionGames)
                .FirstOrDefaultAsync(u => u.Id == predictionGame.GameCreatorId);

            if (gameCreator == null) {
                throw new InvalidOperationException("User not found");
            }

            await context.PredictionGames.AddAsync(predictionGame);
            await context.SaveChangesAsync();

            gameCreator.CreatedPredictionGames.Add(predictionGame);

            var gameCreatorAsParticipant = new PredictionGameParticipant {
                UserId = gameCreator.Id,
                GameId = predictionGame.Id,
                Role = "GameCreator"
            };

            await context.PredictionGameParticipants.AddAsync(gameCreatorAsParticipant);
            await context.SaveChangesAsync();
            return predictionGame;
        }
        //Kuva kasutajaga seotud (created/joined) mänge)
        public async Task<List<PredictionGame>> GetPredictionGamesRelatedWithUser(int userId) {
            IQueryable<PredictionGame> query = context.PredictionGames
            .Where(x => x.Participants.Any(p => p.UserId == userId))
            .Include(m => m.Events)
            .Include(t => t.GameCreator)
            .AsQueryable();

            return await query.ToListAsync();
        }

        //READ
        public async Task<List<PredictionGame>> GetAllPredictionGames()
        {
            IQueryable<PredictionGame> query = context.PredictionGames
            .Include(m => m.Events)
            .Include(m => m.GameCreator)
            .AsQueryable();
            return await query.ToListAsync();
        }

        public async Task<PredictionGame?> GetPredictionGameById(int id) => 
            await context.PredictionGames.Include(m => m.Events).Include(pg => pg.GameCreator).FirstOrDefaultAsync(x => x.Id == id);
        public async Task<bool> PredictionGameExistsInDb(int id) => await context.PredictionGames.AnyAsync(x => x.Id == id);

        //UPDATE
        public async Task<bool> UpdatePredictionGame(int id, PredictionGame predictionGame)
        {
            bool isIdsMatch = predictionGame.Id == id;
            bool gameExists = await PredictionGameExistsInDb(id);

            if (!isIdsMatch || !gameExists)
            {
                return false;
            }

            context.Update(predictionGame);

            int updatedRecordsCount = await context.SaveChangesAsync();
            return updatedRecordsCount > 0;
        }

        public async Task<bool> DeletePredictionGameById(int id)
        {
            PredictionGame? predictionGameInDb = await context.PredictionGames.FindAsync(id);
            if (predictionGameInDb == null)
            {
                return false;
            }

            context.Remove(predictionGameInDb);
            int changesCount = await context.SaveChangesAsync();

            return changesCount == 1;
        }

        public async Task<ApplicationUser?> GetUserById(int userId) => await context.ApplicationUsers.FindAsync(userId); //abimeetod Kasutaja saamiseks.
        public async Task<bool> JoinGameAsync(ApplicationUser user, int gameId){

            bool gameExists = await PredictionGameExistsInDb(gameId);
            if (!gameExists) return false;
            bool isAlreadyInGame = await IsUserAlreadyInGame(user.Id, gameId);
            if (isAlreadyInGame) return false;

            var participant = new PredictionGameParticipant{
                GameId = gameId,
                UserId = user.Id,
                Role = "Player"
            };
            context.PredictionGameParticipants.Add(participant);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> IsUserAlreadyInGame(int userId, int gameId)
        {
            return await context.PredictionGameParticipants.AnyAsync(p => p.UserId == userId && p.GameId == gameId);
        }
        public async Task<PredictionGame?> GetPredictionGameByCode(string uniqueCode)
        {
            return await context.PredictionGames.FirstOrDefaultAsync(g => g.UniqueCode == uniqueCode);
        }
        public async Task<bool> PredictionGameExistsWithCode(string uniqueCode)
        {
            return await context.PredictionGames.AnyAsync(pg => pg.UniqueCode == uniqueCode);
        }
        public async Task<bool> LeaveGameAsync(int userId, int gameId)
        {
        var game = await context.PredictionGames
                             .Include(g => g.Participants)
                             .FirstOrDefaultAsync(g => g.Id == gameId);

        if (game == null) return false;

        var user = game.Participants.FirstOrDefault(u => u.Id == userId);
        if (user == null) return false;

        game.Participants.Remove(user);
        await context.SaveChangesAsync();

        return true;
        }


    }
}