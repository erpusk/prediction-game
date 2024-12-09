using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using itb2203_2024_predictiongame.Backend.Models.Classes;

namespace BackEnd.Models.Classes
{
    public class PredictionGameParticipant
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public PredictionGame? Game { get; set; }

        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public string? Role { get; set; }
        public int EarnedPoints { get; set; } = 0;
    }
}