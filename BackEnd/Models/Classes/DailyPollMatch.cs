using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models.Classes
{
    public class DailyPollMatch
    {
        public int Id { get; set; }
        public string HomeTeamName { get; set; } = string.Empty;
        public string AwayTeamName { get; set; } = string.Empty;
        public DateTime UtcDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsActiveMatch { get; set; }
        public List<PollVote> PollVotes { get; set; } = new List<PollVote>();
    }
}