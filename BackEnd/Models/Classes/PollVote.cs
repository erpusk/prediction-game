using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models.Classes
{
    public class PollVote
    {
        public int Id { get; set; }
        public int DailyPollMatchId { get; set; }
        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public bool IsForHomeTeam { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
    }
}