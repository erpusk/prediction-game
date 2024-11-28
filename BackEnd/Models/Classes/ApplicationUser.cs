
using System.Reflection;
using itb2203_2024_predictiongame.Backend.Models.Classes;
using Microsoft.AspNetCore.Identity;

namespace BackEnd.Models.Classes
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set;} = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public DateTime DateOfBirth { get; set; }
        public List<PredictionGame> CreatedPredictionGames { get; set; } = new List<PredictionGame>();
        //public List<PredictionGame> JoinedPredictionGames { get; set; } = new List<PredictionGame>();
        public List<PredictionGameParticipant> Games { get; set; } = new List<PredictionGameParticipant>();
        public byte[]? ProfilePicture { get; set; }
    }
}