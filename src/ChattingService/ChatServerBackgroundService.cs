using Microsoft.Extensions.Hosting;

namespace ChattingService;

public class ChatServerBackgroundService(IChatServer chatServer) : BackgroundService
{
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await chatServer.StartAsync(cancellationToken);
        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await chatServer.StopAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
        => Task.CompletedTask;
}