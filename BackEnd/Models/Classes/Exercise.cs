using itb2203_2024_predictiongame.Backend.Models.Enums;

namespace itb2203_2024_predictiongame.Backend.Models.Classes
{
    public record Exercise
    {
        public int Id { get; init; }
        public string? Title { get; init; }
        public string? Description { get; init; }
        public ExerciseIntensity Intensity { get; init; }
        public int RecommendedDurationInSeconds { get; init; }
        public int RecDurationBefore { get; init; }
        public int RecDurationAfter { get; init; }
        public string? RestTimeInstructions { get; init; }
    }
}
