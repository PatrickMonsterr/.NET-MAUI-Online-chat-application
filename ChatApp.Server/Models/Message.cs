namespace ChatApp.Server.Models;

public class Message
{
    public int Id { get; set; }
    public string NickName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
