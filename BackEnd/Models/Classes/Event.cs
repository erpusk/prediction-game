namespace BackEnd.Models.Classes
{
    public record Event 
    {
        public int Id { get; set; } 
        public string TeamA { get; set; } = string.Empty; 
        public string TeamB { get; set; } = string.Empty; 
        public DateTime EventDate { get; set; } = DateTime.Now.ToUniversalTime();
        public int PredictionGameId { get; set; } 
        public int? TeamAScore { get; set; } 
        public int? TeamBScore { get; set; } 
        public bool IsCompleted { get; set; } 

    // Predictions (optional, depending on if prediction logic is part of the match)
    // public List<Prediction> Predictions { get; set; } = new List<Prediction>();
    }
}