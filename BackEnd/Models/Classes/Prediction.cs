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
        public int endScoreTeamA { get; set; }
        public int endScoreTeamB { get; set; }
    }
}