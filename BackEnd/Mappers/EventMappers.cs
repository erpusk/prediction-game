using BackEnd.DTOs.Event;
using BackEnd.Models.Classes;

namespace BackEnd.Mappers
{
    public static class EventMappers
    {
        public static EventDto ToEventDto(this Event eventModel) {
            return new EventDto {
                Id = eventModel.Id,
                TeamA = eventModel.TeamA,
                TeamB = eventModel.TeamB,
                EventDate = eventModel.EventDate,
                PredictionGameId = eventModel.PredictionGameId,
                TeamAScore = eventModel.TeamAScore,
                TeamBScore = eventModel.TeamBScore,
                IsCompleted = eventModel.IsCompleted
            };
        }
        public static Event ToEvent(this EventDto eventDto) {
            return new Event {
                Id = eventDto.Id,
                TeamA = eventDto.TeamA,
                TeamB = eventDto.TeamB,
                EventDate = eventDto.EventDate,
                PredictionGameId = eventDto.PredictionGameId,
                TeamAScore = eventDto.TeamAScore,
                TeamBScore = eventDto.TeamBScore,
                IsCompleted = eventDto.IsCompleted
            };
        }

        public static Event ToEventFromCreateDTO(this CreateEventRequestDto eventDto, int predictinoGameId) {
            return new Event {
                TeamA = eventDto.TeamA,
                TeamB = eventDto.TeamB,
                EventDate = eventDto.EventDate,
                PredictionGameId = predictinoGameId,
                TeamAScore = eventDto.TeamAScore,
                TeamBScore = eventDto.TeamBScore,
                IsCompleted = eventDto.IsCompleted
            };
        }

        public static Event ToEventFromUpdateDTO(this UpdateEventRequestDto eventDto) {
            return new Event {
                TeamA = eventDto.TeamA,
                TeamB = eventDto.TeamB,
                EventDate = eventDto.EventDate,
                //PredictionGameId = eventDto.PredictionGameId,
                TeamAScore = eventDto.TeamAScore,
                TeamBScore = eventDto.TeamBScore,
                IsCompleted = eventDto.IsCompleted
            };
        }
    }
}