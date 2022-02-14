using System.Configuration;
using war_game;
using war_game.SMTP;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new Database(builder.Configuration.GetConnectionString("DefaultConnection")));
var smtp = builder.Configuration.GetConnectionString("SMTP").Split('|');
builder.Services.AddSingleton(new SmtpService(smtp[0], smtp[1]));

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
