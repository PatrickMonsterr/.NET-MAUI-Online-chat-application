namespace ChatApp.Client.Models;

public class Message
{
    public string NickName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// True, jeśli wiadomość pochodzi od aktualnie zalogowanego użytkownika (NickName z ViewModelu).
    /// Ustawiane po stronie klienta.
    /// </summary>
    public bool IsMine { get; set; }
}
