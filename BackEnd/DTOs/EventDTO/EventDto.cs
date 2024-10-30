using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOs.Event
{
    public class EventDto
    {
        public int Id { get; set; } 

        [Required]
        public required string TeamA { get; set; } = string.Empty;

        [Required]
        public required string TeamB { get; set; } = string.Empty; 

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EventDate { get; set; } = DateTime.Now.ToUniversalTime();
        public int PredictionGameId { get; set; }
        public int? TeamAScore { get; set; } 
        public int? TeamBScore { get; set; } 
        public bool IsCompleted { get; set; } 

    // Predictions (optional, depending on if prediction logic is part of the match)
        //public List<Prediction> Predictions { get; set; } = new List<Prediction>();
    }
}