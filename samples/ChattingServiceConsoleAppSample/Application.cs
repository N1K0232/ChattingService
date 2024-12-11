using ChattingService.Client;
using ChattingService.Shared.Models;

namespace ChattingServiceConsoleAppSample;

public class Application
{
    public async Task RunAsync()
    {
        Console.Write("Type your message: ");
        string? text = Console.ReadLine();

        using var client = new ChatClient();
        await client.ConnectAsync("127.0.0.1", 5000);

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

        await client.SendAsync(message);
        await client.ReceiveAsync(MessageReceivedAsync);
    }

    private async Task MessageReceivedAsync(ChatMessage message, CancellationToken cancellationToken)
    {
        await Task.Delay(500, cancellationToken);
        string responseMessage = string.Format("Message received: {0}", message.Text);

        Console.WriteLine(responseMessage);
    }
}