using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.DTOs.PollVote
{
    public class PollVoteRequest
    {
        [Required]
        public int DailyPollMatchId { get; set; }

        [Required]
        public bool IsForHomeTeam { get; set; }
    }
}