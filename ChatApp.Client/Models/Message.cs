namespace ChatApp.Client.Models;

public class Message
{
    public string NickName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
