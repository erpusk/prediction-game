using System.ComponentModel.DataAnnotations;
using BackEnd.DTOs.Event;

namespace BackEnd.DTOs.PredictionGame
{
    public class CreatePredictionGameRequestDto
    {
        [Required]
        [MaxLength(40, ErrorMessage = "Title must be less than 40 characters")]
        [MinLength(4, ErrorMessage = "Title must have more than 4 characters")]
        public required string PredictionGameTitle { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; } = DateTime.Now.ToUniversalTime();

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; } = DateTime.Now.ToUniversalTime();

        [Required]
        public string Privacy { get; set; } = "Private game";
        public List<EventDto>? Events { get; set; } = new List<EventDto>();
        // public List<GameParticipant> Participants { get; set; } = new List<GameParticipant>();
        public string? UniqueCode { get; set; }
    }
}