using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models.Classes {
    public class DailyPollMatchDto {
        public int Id { get; set;}
        public required CompetitionDto Competition { get; set; }
        public required TeamDto HomeTeam { get; set; }
        public required TeamDto AwayTeam { get; set; }
        public DateTime UtcDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
    public class TeamDto {
        public string Name { get; set; } = string.Empty;
    }
    public class CompetitionDto {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
    public class RootDto {
        public List<DailyPollMatchDto> Matches { get; set; } = new List<DailyPollMatchDto>();
    }
}