using BackEnd.DTOs.Event;
using BackEnd.DTOs.PredictionGame;
using itb2203_2024_predictiongame.Backend.Models.Classes;

namespace BackEnd.Mappers
{
    public static class PredictionGameMappers
    {
        public static PredictionGameDto ToPredictionGameDto(this PredictionGame predictionGameModel) {
            return new PredictionGameDto {
                Id = predictionGameModel.Id,
                PredictionGameTitle = predictionGameModel.PredictionGameTitle,
                StartDate = predictionGameModel.StartDate,
                EndDate = predictionGameModel.EndDate,
                Privacy = predictionGameModel.Privacy,
                Events = predictionGameModel.Events?.Select(e => e.ToEventDto()).ToList() ?? new List<EventDto>(),
                UniqueCode = predictionGameModel.UniqueCode,
                GameCreatorId = predictionGameModel.GameCreatorId
                // GameCreator = predictionGameModel.GameCreator != null 
                //             ? ApplicationUserMappers.ToApplicationUserDto(predictionGameModel.GameCreator) 
                //             : null,
            };
        }

        public static PredictionGame ToPredictionGameFromCreateDTO(this CreatePredictionGameRequestDto predictionGameDto, int gameCreatorId) {
            return new PredictionGame {
                PredictionGameTitle = predictionGameDto.PredictionGameTitle,
                CreationDate = DateTime.Now,
                StartDate = predictionGameDto.StartDate,
                EndDate = predictionGameDto.EndDate,
                Privacy = predictionGameDto.Privacy,
                Events = predictionGameDto.Events?.Select(e => e.ToEvent()).ToList(),
                UniqueCode = predictionGameDto.UniqueCode,
                GameCreatorId = gameCreatorId
                // GameCreator = predictionGameDto.GameCreator != null
                //                 ? ApplicationUserMappers.ToApplicationUserFromDto(predictionGameDto.GameCreator)
                //                 : null,
            };
        }

        public static PredictionGame ToPredictionGameFromUpdateDTO(this UpdatePredictionGameDto predictionGameDto) {
            return new PredictionGame {
                PredictionGameTitle = predictionGameDto.PredictionGameTitle,
                StartDate = predictionGameDto.StartDate,
                EndDate = predictionGameDto.EndDate,
                Privacy = predictionGameDto.Privacy,
                Events = predictionGameDto.Events?.Select(e => e.ToEvent()).ToList(),
                UniqueCode = predictionGameDto.UniqueCode
                // GameCreator = predictionGameDto.GameCreator != null
                //                 ? ApplicationUserMappers.ToApplicationUserFromDto(predictionGameDto.GameCreator)
                //                 : null,
            };
        }
    }
}