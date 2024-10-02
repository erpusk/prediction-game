using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models.Classes;
using Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data.Repos
{
    public class PredictionGamesRepo(DataContext context)
    {
        private readonly DataContext context = context;
        
        //CREATE
        public async Task<PredictionGame> SavePredictionGameToDb(PredictionGame predictionGame) {
            context.Add(predictionGame);
            await context.SaveChangesAsync();
            return predictionGame;
        }

        //READ
        public async Task<List<PredictionGame>> GetAllPredictionGames() {
            IQueryable<PredictionGame> query = context.PredictionGames.AsQueryable();
            
            return await query.ToListAsync();
        }

        public async Task<PredictionGame?> GetPredictionGameById(int id) => await context.PredictionGames.FindAsync(id);
        public async Task<bool> PredictionGameExistsInDb(int id) => await context.PredictionGames.AnyAsync(x => x.Id == id);

         //UPDATE
        public async Task<bool> UpdatePredictionGame(int id, PredictionGame predictionGame) {
            bool isIdsMatch = predictionGame.Id == id;
            bool gameExists = await PredictionGameExistsInDb(id);

            if (!isIdsMatch || !gameExists) {
                return false;
            }
            
            context.Update(predictionGame);

            int updatedRecordsCount = await context.SaveChangesAsync();
            return updatedRecordsCount == 1;
        }

        public async Task<bool> DeletePredictionGameById(int id) {
            PredictionGame? predictionGameInDb = await GetPredictionGameById(id);
            if (predictionGameInDb == null) {
                return false;
            }

            context.Remove(predictionGameInDb);
            int changesCount = await context.SaveChangesAsync();

            return changesCount == 1;
        }
    }
}