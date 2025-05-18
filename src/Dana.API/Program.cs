using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Setup Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add YARP reverse proxy and load config from appsettings.json
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Map reverse proxy middleware to handle requests
app.MapReverseProxy();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
