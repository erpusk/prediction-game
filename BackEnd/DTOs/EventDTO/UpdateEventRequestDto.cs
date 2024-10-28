namespace BackEnd.DTOs.Event
{
    public class UpdateEventRequestDto
    {
        public string TeamA { get; set; } = string.Empty; 
        public string TeamB { get; set; } = string.Empty; 
        public DateTime EventDate { get; set; } = DateTime.Now.ToUniversalTime();
        public int PredictionGameId { get; set; }
        public int? TeamAScore { get; set; } 
        public int? TeamBScore { get; set; } 
        public bool IsCompleted { get; set; }
    }
}