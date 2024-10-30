using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.DTOs.Prediction
{
    public class CreatePredictionRequestDto
    {
        public int EndScoreTeamA { get; set; }
        public int EndScoreTeamB { get; set; }
        public int EventId { get; set; }
    }
}