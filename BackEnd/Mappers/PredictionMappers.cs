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
                EndScoreTeamA = predictionModel.EndScoreTeamA,
                EndScoreTeamB = predictionModel.EndScoreTeamB,
                PredictionMakerId = predictionModel.PredictionMakerId,
            };
        }

        public static Prediction toPredictionFromCreateDto(this CreatePredictionRequestDto predictionDto){
            return new Prediction {
                EndScoreTeamA = predictionDto.EndScoreTeamA,
                EndScoreTeamB = predictionDto.EndScoreTeamB,
                EventId = predictionDto.EventId
            };
        }  
    }
}