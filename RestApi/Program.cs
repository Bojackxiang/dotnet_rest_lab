// using Serilog;

using Microsoft.EntityFrameworkCore;
using RestApi.enums;
using RestApi.logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// LOGGING
// Log.Logger = new LoggerConfiguration().MinimumLevel
//     .Debug()
//     .WriteTo.File("./log/log.txt", rollingInterval: RollingInterval.Day)
//     .CreateLogger();

// builder.Host.UseSerilog();
builder.Services.AddDbContext<ApplicationDbContext>(optiton =>
{
    optiton.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ILogging, Logging>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();