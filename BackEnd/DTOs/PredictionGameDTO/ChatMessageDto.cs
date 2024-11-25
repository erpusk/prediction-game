using System.ComponentModel.DataAnnotations;

namespace BackEnd.DTOs.PredictionGameDTO;

public class ChatMessageDto
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public string? Sender { get; set; }
    public string? Message { get; set; }
    public string? SenderName { get; set; }
    public DateTime Timestamp { get; set; }
    public int GameId { get; internal set; }
}
