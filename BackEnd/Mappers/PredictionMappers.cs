using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.DTOs.Prediction;
using BackEnd.Models.Classes;

namespace BackEnd.Mappers
{
    public static class PredictionMappers
    {
        public static PredictionDto toPredictionDto(this Prediction predictionModel){
            return new PredictionDto{
                Id = predictionModel.Id,
                endScoreTeamA = predictionModel.endScoreTeamA,
                endScoreTeamB = predictionModel.endScoreTeamB,
                PredictionMakerId = predictionModel.PredictionMakerId,
            };
        }
    }
}