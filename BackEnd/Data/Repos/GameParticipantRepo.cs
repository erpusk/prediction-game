using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models.Classes;
using BackEnd.PointsCalculation;
using itb2203_2024_predictiongame.Backend.Data;
using itb2203_2024_predictiongame.Backend.Models.Classes;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data.Repos
{
    public class GameParticipantRepo
    {
        private readonly DataContext _context;

        public GameParticipantRepo(DataContext context) {
            _context = context;
        }

        public async Task UpdateParticipantPoints(int userId, int predictionGameId, int pointsToAward){
            var participant = await _context.PredictionGameParticipants
            .FirstOrDefaultAsync(gp => gp.UserId == userId && gp.GameId == predictionGameId);

            if (participant != null) {
                participant.EarnedPoints += pointsToAward;
                _context.Update(participant);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AwardPoints(int eventId, int userId){
            var completedEvent = await _context.Events
            .Include(x => x.Predictions)
            .FirstOrDefaultAsync(e => e.Id == eventId && e.IsCompleted == true);
    
            if (completedEvent == null) return;

            var predictionGame = await _context.PredictionGames
            .Include(pg => pg.Events)
            .FirstOrDefaultAsync(pg => pg.Id == completedEvent.PredictionGameId);
            
            if(predictionGame == null) return;

            var userPrediction = completedEvent.Predictions.FirstOrDefault(p => p.PredictionMakerId == userId);
            if (userPrediction == null) return;

            PointsCalculator pointsCalculator = new PointsCalculator(userPrediction, completedEvent);

            var calculatedPoints = pointsCalculator.CalculatePoints();

            await UpdateParticipantPoints(userId, predictionGame.Id, calculatedPoints);
        }

    }
}