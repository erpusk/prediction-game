using Microsoft.EntityFrameworkCore;
using itb2203_2024_predictiongame.Backend.Models.Classes;
using BackEnd.Models.Classes;

namespace itb2203_2024_predictiongame.Backend.Data.Repos
{
    public class PredictionGamesRepo(DataContext context)
    {
        private readonly DataContext context = context;


        //CREATE
        public async Task<PredictionGame> CreatePredictionGameToDb(PredictionGame predictionGame)
        {           
            context.Add(predictionGame);
            await context.SaveChangesAsync();
            return predictionGame;
        }

        //READ
        public async Task<List<PredictionGame>> GetAllPredictionGames()
        {
            IQueryable<PredictionGame> query = context.PredictionGames.Include(m => m.Events).Include(m => m.GameCreator).AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<PredictionGame?> GetPredictionGameById(int id) => await context.PredictionGames.Include(m => m.Events).FirstOrDefaultAsync(x => x.Id == id);
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
    }
}