using Microsoft.Extensions.DependencyInjection;

namespace ChattingService.Server;

public static class ChatServerServiceCollectionExtensions
{
    public static IServiceCollection AddChatServer(this IServiceCollection services, Action<ChatServerOptions> setupAction)
    {
        var options = new ChatServerOptions();
        setupAction.Invoke(options);

        services.AddSingleton(options);
        services.AddSingleton<IChatServer, ChatServer>();

        return services;
    }
}