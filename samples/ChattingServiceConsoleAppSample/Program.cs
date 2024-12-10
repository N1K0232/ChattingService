using ChattingService;
using ChattingServiceConsoleAppSample;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(ConfigureServices)
    .Build();

var application = host.Services.GetRequiredService<Application>();
await host.RunAsync();
await application.ExecuteAsync();

void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    var settings = new ChatSettings
    {
        Port = 5000,
        MaxClientsCount = 10
    };

    services.AddSingleton(settings);

    services.AddSingleton<IChatServer, ChatServer>();
    services.AddSingleton<IChatClient, ChatClient>();

    services.AddSingleton<Application>();
    services.AddHostedService<ChatServerBackgroundService>();
}