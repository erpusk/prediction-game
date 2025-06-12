using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models.Enums;

namespace BackEnd.Models.Classes
{
    public class BonusQuestion
    {
        public int Id { get; set; }
        public string? Question { get; set; }
        public int PredictionGameId { get; set; }
        public BonusQuestionType QuestionType { get; set; }
        public string? OptionsJson { get; set; }
    }
}