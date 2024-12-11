namespace ChattingService.Server.Sample;

public class Application(IChatServer chatServer)
{
    public async Task RunAsync()
    {
        Console.WriteLine("Starting connection . . .");
        await chatServer.StartAsync();

        Console.WriteLine("Press any key to stop the server");
        Console.ReadKey();

        await chatServer.StopAsync();
    }
}