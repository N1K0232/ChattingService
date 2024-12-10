namespace ChattingService.Models;

public class ChatAttachment
{
    public string FileName { get; set; } = null!;

    public Stream Content { get; set; } = null!;
}