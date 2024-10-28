using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.DTOs.PredictionGame;

namespace BackEnd.DTOs.ApplicationUser
{
    public class ApplicationUserDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public DateTime DateOfBirth { get; set; } = DateTime.Now.ToUniversalTime();
        public List<PredictionGameDto> CreatedPredictionGames { get; set; } = new List<PredictionGameDto>();
        //public List<PredictionGameDto> JoinedPredictionGames { get; set; } = new List<PredictionGameDto>();
    }
}