namespace ChattingService.Shared.Models;

public class ChatMessage
{
    public Guid SenderId { get; set; }

    public Guid RecipientId { get; set; }

    public string Text { get; set; } = null!;

    public IList<ChatAttachment>? Attachments { get; set; }
}