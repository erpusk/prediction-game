using BackEnd.DTOs.Event;

namespace BackEnd.DTOs.PredictionGame
{
    public class CreatePredictionGameRequestDto
    {
        public string PredictionGameTitle { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } = DateTime.Now.ToUniversalTime();
        public DateTime EndDate { get; set; } = DateTime.Now.ToUniversalTime();
        //public int GameCreatorId { get; set; }
        //public ApplicationUserDto? GameCreator { get; set; }
        public string Privacy { get; set; } = "Private game";
        public List<EventDto>? Events { get; set; } = new List<EventDto>();
        // public List<GameParticipant> Participants { get; set; } = new List<GameParticipant>();
        public string? UniqueCode { get; set; }
    }
}