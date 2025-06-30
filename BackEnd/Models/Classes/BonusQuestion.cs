using BackEnd.Models.Enums;

namespace BackEnd.Models.Classes
{
    public class BonusQuestion
    {
        public int Id { get; set; }
        public string? Question { get; set; }
        public int PredictionGameId { get; set; }
        public BonusQuestionType QuestionType { get; set; }
        public string? CorrectAnswer { get; set; }
        public List<string>? Options { get; set; }
    }
}