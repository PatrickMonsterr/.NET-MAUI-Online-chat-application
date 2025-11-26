using Microsoft.AspNetCore.SignalR;
using ChatApp.Server.Services;

namespace ChatApp.Server.Hubs;

public class ChatHub : Hub
{
    private readonly ChatService _chatService;

    public ChatHub(ChatService chatService)
    {
        _chatService = chatService;
    }

    public override async Task OnConnectedAsync()
    {
        // Poinformuj innych, że ktoś się połączył
        await Clients.Others.SendAsync("UserConnected", $"Użytkownik dołączył do czatu");
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(string nickName, string message)
    {
        if (string.IsNullOrWhiteSpace(nickName) || string.IsNullOrWhiteSpace(message))
            return;

        // Zapisz do bazy
        await _chatService.SaveMessageAsync(nickName, message);

        // Wyślij do wszystkich klientów
        await Clients.All.SendAsync("ReceiveMessage", nickName, message, DateTime.UtcNow);
    }

    public async Task<List<object>> GetChatHistory()
    {
        var messages = await _chatService.GetRecentMessagesAsync();
        return messages.Select(m => new { m.NickName, m.Content, m.Timestamp } as object).ToList();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Clients.Others.SendAsync("UserDisconnected", "Użytkownik opuścił czat");
        await base.OnDisconnectedAsync(exception);
    }
}
