using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models.Classes;
using itb2203_2024_predictiongame.Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data.Repos
{
    public class AnswerRepo
    {
        private readonly DataContext _context;

        public AnswerRepo(DataContext context){
            _context = context;
        }

        public async Task<List<Answer>> GetAnswers(int? questionId= null, int? userId = null){
            IQueryable<Answer> answers = _context.Answers.AsQueryable();
            if (questionId.HasValue){
                answers = answers.Where(x => x.QuestionId == questionId);
            }
            if (userId.HasValue){
                answers = answers.Where(x => x.AnswerMakerId == userId);
            }
            return await answers.ToListAsync();
        }

        public async Task<Answer?> GetAnswer(int Id){
            var answer = await _context.Answers.FirstOrDefaultAsync(x => x.Id == Id);
            return answer;
        }

        public async Task<Answer> AddAnswer(Answer answerModel){
            await _context.AddAsync(answerModel);
            await _context.SaveChangesAsync();
            return answerModel;
        }

        public async Task<List<Answer>>GetAnswerByQuestionId(int questionId){
            var answers = await _context.Answers.Where(x => x.QuestionId == questionId).ToListAsync();
            return answers;
        }
    }
}