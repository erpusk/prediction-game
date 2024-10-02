namespace itb2203_2024_predictiongame.Backend.Models.Classes
{
    public record PredictionGame
    {
        public int Id { get; set; }
        public string PredictionGameTitle { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int GameCreatorId { get; set; }
        // public ApplicationUser GameCreator { get; set; }
        public string Privacy { get; set; } = "Private game";
        // public List<Match> Matches { get; set; } = new List<Match>();
        // public List<GameParticipant> Participants { get; set; } = new List<GameParticipant>();
    }
}