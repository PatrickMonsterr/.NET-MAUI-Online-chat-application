using Microsoft.AspNetCore.SignalR.Client;
using ChatApp.Client.Models;

namespace ChatApp.Client.Services;

public class ChatHubClient
{
    private HubConnection? _connection;
    public event Func<string, string, DateTime, Task>? OnMessageReceived;
    public event Func<Task>? OnConnected;
    public event Func<Task>? OnDisconnected;

    public async Task ConnectAsync(string serverUrl)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl($"{serverUrl}/chatHub")
            .WithAutomaticReconnect()
            .Build();

        _connection.On<string, string, DateTime>("ReceiveMessage", async (nickName, message, timestamp) =>
        {
            if (OnMessageReceived != null)
                await OnMessageReceived(nickName, message, timestamp);
        });

        _connection.On("UserConnected", async () =>
        {
            System.Diagnostics.Debug.WriteLine("Użytkownik dołączył");
        });

        _connection.On("UserDisconnected", async () =>
        {
            System.Diagnostics.Debug.WriteLine("Użytkownik opuścił");
        });

        _connection.On<string>("UserConnected", async (msg) =>
        {
            System.Diagnostics.Debug.WriteLine($"Użytkownik dołączył: {msg}");
        });

        _connection.On<string>("UserDisconnected", async (msg) =>
        {
            System.Diagnostics.Debug.WriteLine($"Użytkownik opuścił: {msg}");
        });

        try
        {
            await _connection.StartAsync();
            if (OnConnected != null)
                await OnConnected();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Błąd połączenia: {ex.Message}");
        }
    }

    public async Task SendMessageAsync(string nickName, string message)
    {
        if (_connection?.State == HubConnectionState.Connected)
        {
            try
            {
                await _connection.InvokeAsync("SendMessage", nickName, message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Błąd wysyłania: {ex.Message}");
            }
        }
    }


    public async Task DisconnectAsync()
    {
        if (_connection != null)
        {
            await _connection.StopAsync();
            await _connection.DisposeAsync();
        }
    }

    public bool IsConnected => _connection?.State == HubConnectionState.Connected;
}
