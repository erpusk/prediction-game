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

        public async Task<PredictionGameParticipant?> GetGameParticipantByUserId(int userId) 
        => await _context.PredictionGameParticipants.FirstOrDefaultAsync(p => p.UserId == userId);

        public async Task<bool> IsParticipantExistsInDb(int id) => await _context.PredictionGameParticipants.AnyAsync(x => x.Id == id);

        public async Task<bool> GetParticipantEarnedPoints(int id) {
            bool participantExist = await IsParticipantExistsInDb(id);
            
            if(!participantExist) {
                return false;
            }

            var result = _context.PredictionGameParticipants.Where(x => x.Id == id).Select(x => x.EarnedPoints);

            return result != null;     
        }


        public async Task<bool> UpdateGameParticipant(int id, PredictionGameParticipant participant){
            bool isIdsMatch = id == participant.Id;
            bool participantExistsInDB = await IsParticipantExistsInDb(id);

            if (!isIdsMatch || !participantExistsInDB){
                return false;
            }
            
            _context.Update(participant);
            int changesCount = await _context.SaveChangesAsync();
            return changesCount == 1;
        }

        public async Task<bool> UpdateGameParticipantPoints(int userId, int predictionGameId, int pointsToAward){
             var participant = await _context.PredictionGameParticipants
            .FirstOrDefaultAsync(gp => gp.UserId == userId && gp.GameId == predictionGameId);

            if (participant == null) return false;
   
            participant.EarnedPoints += pointsToAward;
            
            var result = await UpdateGameParticipant(participant.Id, participant);

            return result;
        }

        public async Task<bool> AwardPoints(int eventId, int userId){
            var completedEvent = await _context.Events
            .Include(x => x.Predictions)
            .FirstOrDefaultAsync(e => e.Id == eventId && e.IsCompleted == true);
    
            if (completedEvent == null) return false;

            var predictionGame = await _context.PredictionGames
            .FirstOrDefaultAsync(pg => pg.Id == completedEvent.PredictionGameId);
            
            if(predictionGame == null) return false;

            var calculatedPoints = GetCalculatedPoints(completedEvent, userId);
            if (calculatedPoints == null) return false;

            var result = await UpdateGameParticipantPoints(userId, predictionGame.Id, (int)calculatedPoints);

            return result;
        }

        public async Task<int?> GetParticipantEarnedPointsForEvent(int userId, int eventId) {
            var completedEvent = await _context.Events
            .Include(x => x.Predictions)
            .FirstOrDefaultAsync(e => e.Id == eventId && e.IsCompleted == true);
    
            if (completedEvent == null) return null;

            var earnedPoints = GetCalculatedPoints(completedEvent, userId);
            return earnedPoints;
        }

        private int? GetCalculatedPoints(Event completedEvent, int userId) {
            var userPrediction = completedEvent.Predictions.FirstOrDefault(p => p.PredictionMakerId == userId);
            if (userPrediction == null) return null;

            PointsCalculator pointsCalculator = new PointsCalculator();

            var calculatedPoints = pointsCalculator.CalculatePoints(completedEvent, userPrediction);
            return calculatedPoints;
        }
    }
}