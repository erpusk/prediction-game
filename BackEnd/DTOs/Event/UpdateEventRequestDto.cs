using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOs.Event
{
    public class UpdateEventRequestDto
    {
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
    }
}