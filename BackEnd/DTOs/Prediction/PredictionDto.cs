using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.DTOs.Prediction
{
    public class PredictionDto
    {
        public int Id { get; set; }
        public int PredictionMakerId { get; set; }
        public int endScoreTeamA { get; set; }
        public int endScoreTeamB { get; set; }
    }
}