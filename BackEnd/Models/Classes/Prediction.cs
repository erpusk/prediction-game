using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models.Classes
{
    public record Prediction
    {
        public int Id { get; set; }
        public int PredictionMakerId { get; set; }
        public int EventId { get; set; }
        public int EndScoreTeamA { get; set; }
        public int EndScoreTeamB { get; set; }
        public int PointsEarned { get; set; } = 0;
    }
}