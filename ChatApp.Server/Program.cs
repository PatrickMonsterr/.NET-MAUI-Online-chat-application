using ChatApp.Server.Hubs;

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

// 2. SignalR (do komunikacji w czasie rzeczywistym)
builder.Services.AddSignalR();

// 3. Kontrolery (jeśli kiedyś dodasz REST API – mogą zostać)
builder.Services.AddControllers();

var app = builder.Build();

// 4. Włączamy CORS
app.UseCors("AllowAll");

app.UseRouting();

// 5. Endpoint dla SignalR
app.MapHub<ChatHub>("/chatHub");

app.MapControllers();

app.Run();
