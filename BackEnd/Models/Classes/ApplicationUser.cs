
using itb2203_2024_predictiongame.Backend.Models.Classes;
using Microsoft.AspNetCore.Identity;

namespace BackEnd.Models.Classes
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set;} = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public DateTime DateOfBirth { get; set; } = DateTime.Now.ToUniversalTime();
        public List<PredictionGame>? CreatedPredictionGames { get; set; } = new List<PredictionGame>();
        //public List<PredictionGame> JoinedPredictionGames { get; set; } = new List<PredictionGame>();
    }
}