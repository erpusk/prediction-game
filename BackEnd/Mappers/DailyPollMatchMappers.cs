using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models.Classes;

namespace BackEnd.Mappers
{
    public static class DailyPollMatchMappers
    {
        public static DailyPollMatch ToDailyPollMatch(this DailyPollMatchDto dto) {
            return new DailyPollMatch {
                Id = dto.Id,
                HomeTeamName = dto.HomeTeam.Name,
                AwayTeamName = dto.AwayTeam.Name,
                UtcDate = dto.UtcDate,
                Status = dto.Status,
            };
        }
    }
}