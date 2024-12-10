using ChattingService.Models;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ChattingService;

public class ChatClient : IChatClient
{
    private TcpClient client = new();
    private NetworkStream stream = null!;

    private bool disposed = false;

    public async Task ConnectAsync(string serverIp, int port, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        try
        {
            await client.ConnectAsync(serverIp, port);
            stream = client.GetStream();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task SendAsync(ChatMessage message, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        string content = JsonSerializer.Serialize(message);

        byte[] buffer = Encoding.UTF8.GetBytes(content);
        await stream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
    }

    public async Task ReceiveAsync(Func<ChatMessage, CancellationToken, Task> onMessageReceived, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        var buffer = new byte[1024];

        while (client.Connected)
        {
            int read = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
            if (read > 0)
            {
                string content = Encoding.UTF8.GetString(buffer, 0, read);
                var message = JsonSerializer.Deserialize<ChatMessage>(content)!;

                await onMessageReceived(message, cancellationToken);
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (!disposed)
            {
                client.Close();
                client.Dispose();

                stream.Close();
                stream.Dispose();

                client = null!;
                stream = null!;

                disposed = true;
            }
        }
    }

    private void ThrowIfDisposed()
    {
        if (disposed)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
    }
}