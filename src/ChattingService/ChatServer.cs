using ChattingService.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ChattingService;

public class ChatServer(ChatSettings options) : IChatServer
{
    private TcpListener listener = new(IPAddress.Any, options.Port);
    private IList<TcpClient> clients = [];

    private bool disposed = false;

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        int clientCount = 0;

        listener.Start();
        Console.WriteLine("Server started listening");

        while (clientCount < options.MaxClientsCount)
        {
            var client = await listener.AcceptTcpClientAsync();
            lock (clients)
            {
                clients.Add(client);
            }

            await HandleClientAsync(client, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        listener.Stop();
        return Task.CompletedTask;
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
                listener.Stop();
                listener = null!;

                clients.Clear();
                clients = null!;

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

    private async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken = default)
    {
        NetworkStream stream = client.GetStream();
        var buffer = new byte[1024];

        try
        {
            while (client.Connected)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    string json = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    var message = JsonSerializer.Deserialize<ChatMessage>(json)!;

                    await BroadcastMessageAsync(message, cancellationToken);
                }
            }
        }
        catch (Exception ex)
        {
            lock (clients)
            {
                clients.Remove(client);
            }

            Console.WriteLine(ex.Message);
        }
        finally
        {
            await stream.DisposeAsync();
        }
    }

    private async Task BroadcastMessageAsync(ChatMessage message, CancellationToken cancellationToken = default)
    {
        string content = JsonSerializer.Serialize(message);
        byte[] buffer = Encoding.UTF8.GetBytes(content);

        foreach (TcpClient client in clients)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                await stream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
            }
            catch (Exception ex)
            {
                lock (clients)
                {
                    clients.Remove(client);
                }

                Console.WriteLine(ex.Message);
            }
        }
    }
}