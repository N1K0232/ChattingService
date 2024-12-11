using ChattingService.Server;
using ChattingService.Server.Sample;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(ConfigureServices)
    .Build();

var application = host.Services.GetRequiredService<Application>();
await application.RunAsync();

void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    services.AddChatServer(options =>
    {
        options.Port = 5000;
        options.MaxClientsCount = 10;
    });

    services.AddSingleton<Application>();
}