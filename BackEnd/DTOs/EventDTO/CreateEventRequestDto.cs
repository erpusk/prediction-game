using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOs.Event
{
    public class CreateEventRequestDto
    {
        [Required]
        public required string TeamA { get; set; }

        [Required]
        public required string TeamB { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EventDate { get; set; } = DateTime.Now.ToUniversalTime();
        //public int PredictionGameId { get; set; }
        public bool IsCompleted { get; set; } 
    }
}