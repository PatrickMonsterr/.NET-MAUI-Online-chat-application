using ChatApp.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

// 1. Konfiguracja CORS 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 2. SignalR
builder.Services.AddSignalR();

builder.Services.AddControllers();
var app = builder.Build();

app.UseCors("AllowAll");

app.UseRouting();

// 5. Endpoint dla SignalR
app.MapHub<ChatHub>("/chatHub");

app.MapControllers();

app.Run();
