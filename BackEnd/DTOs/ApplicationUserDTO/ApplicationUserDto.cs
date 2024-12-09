using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.DTOs.PredictionGame;

namespace BackEnd.DTOs.ApplicationUser
{
    public class ApplicationUserDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public int? UserId { get; set;}

        [Required(ErrorMessage = "Please select your date of birth")]
        public DateTime DateOfBirth { get; set; } = DateTime.Now.ToUniversalTime();
        public List<PredictionGameDto> CreatedPredictionGames { get; set; } = new List<PredictionGameDto>();
        //public List<PredictionGameDto> JoinedPredictionGames { get; set; } = new List<PredictionGameDto>();
        public string? ProfilePicture { get; set; }
        public string? Email { get; set; }
    }
}