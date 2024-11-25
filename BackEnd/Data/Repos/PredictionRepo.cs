using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.DTOs.Prediction;
using BackEnd.Models.Classes;
using itb2203_2024_predictiongame.Backend.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data.Repos
{
    public class PredictionRepo
    {
        private readonly DataContext _context;

        public PredictionRepo(DataContext context){
            _context = context;
        }

        public async Task<List<Prediction>> GetAll(int? eventId= null, int? userId = null){
            IQueryable<Prediction> predictions = _context.Predictions.AsQueryable();
            if (eventId.HasValue){
                predictions = predictions.Where(x => x.EventId == eventId);
            }
            if (userId.HasValue){
                predictions = predictions.Where(x => x.PredictionMakerId == userId);
            }
            return await predictions.ToListAsync();
        }

        public async Task<Prediction?> GetById(int id){
            var prediction = await _context.Predictions.FirstOrDefaultAsync(x => x.Id == id);
            return prediction;
        }

        public async Task<Event?> GetPredictionsEvent(Prediction prediction){
            var predictionsEvent = await _context.Events.FirstOrDefaultAsync(x => x.Id == prediction.EventId);
            return predictionsEvent;
        }

        public async Task<Prediction> CreatePrediction(Prediction prediction){
            await _context.Predictions.AddAsync(prediction);
            await _context.SaveChangesAsync();
            return prediction;
        }

        public async Task<List<Prediction>> GetUserPredictionHistory(int userId) {
            var eventsWithFinalScores = await _context.Events
                .Where(e => e.TeamAScore.HasValue && e.TeamBScore.HasValue)
                .Select(e => e.Id)
                .ToListAsync();

            IQueryable<Prediction> query = _context.Predictions
            .Where(p => p.PredictionMakerId == userId && p.EndScoreTeamA.HasValue && p.EndScoreTeamB.HasValue && eventsWithFinalScores.Contains(p.EventId))
            .AsQueryable();

            return await query.ToListAsync();
        }
    }
}