using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.DTOs.Prediction
{
    public class CreatePredictionRequestDto
    {
        public int endScoreTeamA { get; set; }
        public int endScoreTeamB { get; set; }
        public int eventId { get; set; }
    }
}