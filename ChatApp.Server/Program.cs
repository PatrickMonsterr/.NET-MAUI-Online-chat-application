using ChatApp.Server.Data;
using ChatApp.Server.Hubs;
using ChatApp.Server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Konfiguracja CORS (pozwala łączyć się z każdego miejsca)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 2. Baza danych SQLite
builder.Services.AddDbContext<ChatDbContext>(options =>
    options.UseSqlite($"Data Source={Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "chat.db")})"));

// 3. SignalR (do komunikacji w czasie rzeczywistym)
builder.Services.AddSignalR();

// 4. Nasz serwis do logiki czatu
builder.Services.AddScoped<ChatService>();

// 5. Kontrolery (opcjonalnie, jeśli chciałbyś dodać REST API)
builder.Services.AddControllers();

var app = builder.Build();

// 6. Włączamy CORS
app.UseCors("AllowAll");

// 7. Automatyczne tworzenie bazy danych przy starcie (Zastępuje 'dotnet ef database update')
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
    db.Database.EnsureCreated();
}

app.UseRouting();

// 8. Endpoint dla SignalR
app.MapHub<ChatHub>("/chatHub");

app.MapControllers();

app.Run("http://0.0.0.0:5022");
