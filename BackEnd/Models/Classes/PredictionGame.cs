using BackEnd.Models.Classes;
using RandomString4Net;

namespace itb2203_2024_predictiongame.Backend.Models.Classes
{
    public record PredictionGame
    {
        public int Id { get; set; }
        public string PredictionGameTitle { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = DateTime.Now.ToUniversalTime();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int GameCreatorId { get; set; }
        public ApplicationUser? GameCreator { get; set; }
        public string Privacy { get; set; } = "Private game";
        public List<Event>? Events { get; set; } = new List<Event>();
        public string? UniqueCode { get; set; } // = RandomString.GetString(Types.ALPHABET_LOWERCASE, 6);
        public ICollection<PredictionGameParticipant> Participants { get; set; } = new List<PredictionGameParticipant>();
    }
}