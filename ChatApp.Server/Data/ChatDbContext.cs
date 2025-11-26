using Microsoft.EntityFrameworkCore;
using ChatApp.Server.Models;

namespace ChatApp.Server.Data;

public class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
    {
    }

    public DbSet<Message> Messages { get; set; } = null!;
}
