using ChattingService;
using ChattingService.Models;

namespace ChattingServiceConsoleAppSample;

public class Application
{
    public async Task ExecuteAsync()
    {
        Console.WriteLine("Connecting . . .");

        using var primaryClient = new ChatClient();
        await primaryClient.ConnectAsync("127.0.0.1", 5000);

        using var secondaryClient = new ChatClient();
        await secondaryClient.ConnectAsync("127.0.0.1", 5000);

        Console.Write("Type your message: ");
        string? text = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        var message = new ChatMessage
        {
            Text = text,
            SenderId = Guid.NewGuid(),
            RecipientId = Guid.NewGuid()
        };

        await primaryClient.SendAsync(message);
        await secondaryClient.ReceiveAsync(SecondaryClientReceivedAsync);
    }

    private async Task SecondaryClientReceivedAsync(ChatMessage message, CancellationToken cancellationToken)
    {
        await Task.Delay(500, cancellationToken);
        string responseMessage = string.Format("Message received: {0}", message.Text);

        Console.WriteLine(responseMessage);
    }
}