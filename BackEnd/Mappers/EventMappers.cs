using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}