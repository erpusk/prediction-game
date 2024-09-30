using System.Text.Json.Serialization;

namespace WorkoutApplication.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ExerciseIntensity {
        Low=1,
        Normal=2,
        High=3
    }
}
