using MagicOnion.Serialization;
using MagicOnion.Serialization.MemoryPack;
using PerformanceTest.Server;

// Setup serializer
if (Array.IndexOf(args, "--serialization") is var index and > -1 && args[index + 1].ToLowerInvariant() == "memorypack")
{
    MagicOnionSerializerProvider.Default = MemoryPackMagicOnionSerializerProvider.Instance;
}

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddCommandLine(args, new Dictionary<string, string>()
{
    // kestrel
    { "-u", "Url" },
    { "--protocol", "Protocol" },
    { "--client-auth", "ClientAuth" },
    // datadog
    { "--tags", "Tags" },
    { "--validate", "Validate" },
});

builder.Logging.ClearProviders();

// HTTP/HTTPS Configuration
builder.ConfigureEndpoint();

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddMagicOnion();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddHostedService<StartupService>();
builder.Services.AddHostedService<ProfileService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapMagicOnionService();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
