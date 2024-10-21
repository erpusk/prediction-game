using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<List<Prediction>> GetAll(){
            var predictions = await _context.Predictions.ToListAsync();
            return predictions;
        }
    }
}