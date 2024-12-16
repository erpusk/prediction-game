using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models.Classes
{
    public class BonusQuestionAnswer
    {
        public int Id { get; set; }
        public string? AnswerText { get; set; }
        public int QuestionId { get; set; }
        public int AnswerMakerId { get; set; }
    }
}