using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOs.PredictionGameDTO;

public class ChatMessageDto
{
    public int Id { get; set; }
    public string? Sender { get; set; }
    public string? Message { get; set; }
    public DateTime Timestamp { get; set; }
}
