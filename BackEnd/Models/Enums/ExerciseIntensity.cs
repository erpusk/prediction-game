using System.Text.Json.Serialization;

namespace itb2203_2024_predictiongame.Backend.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ExerciseIntensity
    {
        Low = 1,
        Normal = 2,
        High = 3
    }
}
