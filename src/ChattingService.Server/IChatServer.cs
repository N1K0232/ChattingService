namespace ChattingService.Server;

public interface IChatServer : IDisposable
{
    Task StartAsync(CancellationToken cancellationToken = default);

    Task StopAsync(CancellationToken token = default);
}