using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ChatApp.Client.Models;
using ChatApp.Client.Services;

namespace ChatApp.Client.ViewModels;

public partial class ChatViewModel : ObservableObject
{
    private readonly ChatHubClient _hubClient;

    [ObservableProperty]
    private string? nickName = string.Empty;

    [ObservableProperty]
    private string? messageText = string.Empty;

    [ObservableProperty]
    private bool isConnected = false;

    [ObservableProperty]
    private string? statusMessage = "Nie połączono";

    [ObservableProperty]
    private ObservableCollection<Message> messages = new();

    public ChatViewModel()
    {
        _hubClient = new ChatHubClient();
        _hubClient.OnMessageReceived += OnMessageReceived;
        _hubClient.OnConnected += OnConnected;
        _hubClient.OnDisconnected += OnDisconnected;
    }

    [RelayCommand]
    public async Task Connect()
    {
        if (string.IsNullOrWhiteSpace(NickName))
        {
            StatusMessage = "Wpisz nick!";
            return;
        }

        // Jeśli już połączony, to rozłącz
        if (IsConnected)
        {
            await Disconnect();
            return;
        }

        try
        {
            StatusMessage = "Łączenie...";

            // UWAGA: Sprawdź w terminalu serwera, jaki masz port!
            // Zwykle jest to http://localhost:5xxx
            // Spróbujmy użyć obu najpopularniejszych:
            string url = "http://localhost:5022";

            // Jeśli serwer wyświetlał np. 5234, wpisz tutaj 5234!

            await _hubClient.ConnectAsync(url);
        }
        catch (Exception ex)
        {
            StatusMessage = $"Błąd: {ex.Message}";
        }
    }


    [RelayCommand]
    public async Task SendMessage()
    {
        if (string.IsNullOrWhiteSpace(MessageText))
            return;

        try
        {
            await _hubClient.SendMessageAsync(NickName!, MessageText);
            MessageText = string.Empty;
        }
        catch (Exception ex)
        {
            StatusMessage = $"Błąd wysyłania: {ex.Message}";
        }
    }

    private async Task OnMessageReceived(string nickName, string content, DateTime timestamp)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Messages.Add(new Message
            {
                NickName = nickName,
                Content = content,
                Timestamp = timestamp
            });
        });
    }

    private async Task OnConnected()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            IsConnected = true;
            StatusMessage = $"Połączono jako {NickName}";
            Messages.Clear();
        });

        var history = await _hubClient.GetChatHistoryAsync();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            foreach (var msg in history)
            {
                Messages.Add(msg);
            }
        });
    }

    private async Task OnDisconnected()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            IsConnected = false;
            StatusMessage = "Rozłączono";
        });
    }

    public async Task Disconnect()
    {
        await _hubClient.DisconnectAsync();
    }
}
