using BackEnd.DTOs.ApplicationUser;
using BackEnd.DTOs.Event;
using BackEnd.DTOs.PredictionGame;
using BackEnd.DTOs.PredictionGameDTO;
using BackEnd.Models.Classes;
using itb2203_2024_predictiongame.Backend.Models.Classes;

namespace BackEnd.Mappers
{
    public static class PredictionGameMappers
    {
        public static PredictionGameDto ToPredictionGameDto(this PredictionGame predictionGameModel, int currentUserId) {
            bool isUserGameCreator = predictionGameModel.GameCreatorId == currentUserId;
    bool    isUserParticipant = predictionGameModel.Participants!.Any(p => p.UserId == currentUserId);
            return new PredictionGameDto {
                
                Id = predictionGameModel.Id,
                PredictionGameTitle = predictionGameModel.PredictionGameTitle,
                StartDate = predictionGameModel.StartDate,
                EndDate = predictionGameModel.EndDate,
                Privacy = predictionGameModel.Privacy,
                Events = predictionGameModel.Events?.Select(e => e.ToEventDto()).ToList() ?? new List<EventDto>(),
                UniqueCode = (isUserGameCreator || isUserParticipant) ? predictionGameModel.UniqueCode : null,
                GameCreatorId = predictionGameModel.GameCreatorId,
                GameCreator = predictionGameModel.GameCreator?.ToApplicationUserDto(),
                CreationDate = predictionGameModel.CreationDate,
                Participants = predictionGameModel.Participants!
                .Select(p => new ApplicationUserDto
                {
                    Id = p.UserId,
                    UserName = p.User?.UserName ?? "Unknown User",
                    UserId = p.UserId,
                    ProfilePicture = p.User?.ProfilePicture ?? ""
                }).ToList(),
                ChatMessages = predictionGameModel.ChatMessages
                .Select(chat => new ChatMessageDto
                {
                    Id = chat.Id,
                    GameId = chat.GameId,
                    SenderName = chat.Sender != null ? chat.Sender!.UserName : "Unknown User",
                    Message = chat.Message,
                    Timestamp = chat.Timestamp
                }).ToList()
            };
        }

        public static PredictionGame ToPredictionGameFromCreateDTO(this CreatePredictionGameRequestDto predictionGameDto, ApplicationUser gameCreator, string uniqueCode) {
            return new PredictionGame {
                PredictionGameTitle = predictionGameDto.PredictionGameTitle,
                CreationDate = DateTime.Now.ToUniversalTime(),
                StartDate = predictionGameDto.StartDate,
                EndDate = predictionGameDto.EndDate,
                Privacy = predictionGameDto.Privacy,
                Events = predictionGameDto.Events?.Select(e => e.ToEvent()).ToList(),
                UniqueCode = uniqueCode,
                GameCreatorId = gameCreator.Id,
                GameCreator = gameCreator
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
            };
        }
    }
}