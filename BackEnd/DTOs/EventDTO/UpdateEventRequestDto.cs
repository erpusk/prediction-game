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
        public DateTime EventDate { get; set; }
        //public int PredictionGameId { get; set; }
        public int? TeamAScore { get; set; } = null;
        public int? TeamBScore { get; set; } = null;
        public bool IsCompleted { get; set; }
    }
}