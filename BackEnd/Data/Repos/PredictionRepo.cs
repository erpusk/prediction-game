using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.DTOs.Prediction;
using BackEnd.Models.Classes;
using itb2203_2024_predictiongame.Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data.Repos
{
    public class PredictionRepo
    {
        private readonly DataContext _context;

        public PredictionRepo(DataContext context){
            _context = context;
        }

        public async Task<List<Prediction>> GetAll(int? eventId = null){
            IQueryable<Prediction> predictions = _context.Predictions.AsQueryable();
            if (eventId.HasValue){
                predictions = predictions.Where(x => x.EventId == eventId);
            }
            return await predictions.ToListAsync();
        }

        public async Task<Prediction?> GetById(int id){
            var prediction = await _context.Predictions.FirstOrDefaultAsync(x => x.Id == id);
            return prediction;
        }

        public async Task<Prediction> CreatePrediction(Prediction prediction){
            await _context.Predictions.AddAsync(prediction);
            await _context.SaveChangesAsync();
            return prediction;
        }
    }
}