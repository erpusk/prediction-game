using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.DTOs.Event;

namespace BackEnd.DTOs.PredictionGame
{
    public class UpdatePredictionGameDto
    {
        public string PredictionGameTitle { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //public int GameCreatorId { get; set; }
        //public ApplicationUserDto? GameCreator { get; set; }
        public string Privacy { get; set; } = "Private game";
        public List<EventDto>? Events { get; set; } = new List<EventDto>();
        // public List<GameParticipant> Participants { get; set; } = new List<GameParticipant>();
        public string? UniqueCode { get; set; }
    }
}