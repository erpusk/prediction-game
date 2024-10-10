using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.DTOs.Event
{
    public class CreateEventRequestDto
    {
        public string TeamA { get; set; } = string.Empty; 
        public string TeamB { get; set; } = string.Empty; 
        public DateTime EventDate { get; set; } 
        public int PredictionGameId { get; set; }
        public int? TeamAScore { get; set; } 
        public int? TeamBScore { get; set; } 
        public bool IsCompleted { get; set; } 
    }
}