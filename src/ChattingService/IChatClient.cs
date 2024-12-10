using ChattingService.Models;

namespace ChattingService;

public interface IChatClient : IDisposable
{
    Task ConnectAsync(string serverIp, int port, CancellationToken cancellationToken = default);

    Task SendAsync(ChatMessage message, CancellationToken cancellationToken = default);

    Task ReceiveAsync(Func<ChatMessage, CancellationToken, Task> onMessageReceived, CancellationToken cancellationToken = default);
}