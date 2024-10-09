
using Microsoft.AspNetCore.Identity;

namespace BackEnd.Models.Classes
{
    public class ApplicationUser : IdentityUser<int>
    {
        public DateTime DateOfBirth { get; set; }
        //public List<PredictionGame> CreatedPredictionGames { get; set; } = new List<PredictionGame>();
        //public List<PredictionGame> JoinedPredictionGames { get; set; } = new List<PredictionGame>();
    }
}