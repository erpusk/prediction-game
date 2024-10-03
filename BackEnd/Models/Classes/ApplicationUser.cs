using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using itb2203_2024_predictiongame.Backend.Models.Classes;
using Microsoft.AspNetCore.Identity;

namespace BackEnd.Models.Classes
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime DateOfBirth { get; set; }
        public List<PredictionGame> CreatedPredictionGames { get; set; } = new List<PredictionGame>();
        public List<PredictionGame> JoinedPredictionGames { get; set; } = new List<PredictionGame>();
    }
}