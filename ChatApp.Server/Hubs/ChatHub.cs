using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Server.Hubs;

public class ChatHub : Hub
{

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

        // Wyślij do wszystkich klientów
        await Clients.All.SendAsync("ReceiveMessage", nickName, message, DateTime.UtcNow);
    }



    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Clients.Others.SendAsync("UserDisconnected", "Użytkownik opuścił czat");
        await base.OnDisconnectedAsync(exception);
    }
}
