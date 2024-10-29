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
        public int EndScoreTeamA { get; set; }
        public int EndScoreTeamB { get; set; }
    }
}