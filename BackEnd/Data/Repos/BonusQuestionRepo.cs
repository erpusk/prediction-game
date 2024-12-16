using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models.Classes;
using itb2203_2024_predictiongame.Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data.Repos
{
    public class BonusQuestionRepo
    {
        private readonly DataContext _context;

        public BonusQuestionRepo(DataContext context){
            _context = context;
        }

        public async Task<List<BonusQuestion>> GetBonusQuestions(){
            var BonusQuestions = await _context.BonusQuestions.ToListAsync();
            return BonusQuestions;
        }

        public async Task<BonusQuestion?> GetBonusQuestion(int Id){
            var BonusQuestion = await _context.BonusQuestions.FirstOrDefaultAsync(x => x.Id == Id);
            return BonusQuestion;
        }

        public async Task<BonusQuestion> AddBonusQuestion(BonusQuestion bonusQuestion){
            await _context.AddAsync(bonusQuestion);
            await _context.SaveChangesAsync();
            return bonusQuestion;
        }

        public async Task<List<BonusQuestion>>GetPredictionGamesQuestions(int predictionGameId){
            var bonusQuestions = await _context.BonusQuestions.Where(x => x.PredictionGameId == predictionGameId).ToListAsync();
            return bonusQuestions;
        }
    }
}