using SignalCluster.Core;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy
    .WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
    .AllowAnyHeader()
    .AllowAnyMethod()));

var app = builder.Build();
app.UseCors();

app.MapGet("/", () => Results.Ok(new { name = "SignalCluster.Service", version = "0.2.0", mode = "simulation-only" }));
app.MapGet("/health", () => Results.Ok(new { status = "healthy", utc = DateTimeOffset.UtcNow }));
app.MapGet("/build-info", () => Results.Ok(new { product = "SignalCluster", buildName = "SignalCluster Virtual Observatory Simulator", buildId = "SC-SIM-2026.10.29-001", version = "0.2.0", developer = "ArkansasIo", targetRuntime = ".NET 8", hardwareMode = "simulation-only" }));
app.MapGet("/radio/status", () => Results.Ok(new { deviceName = "SignalCluster Virtual Radio Receiver (SC-VRR-01)", simulatedOnly = true, rfInputConnected = false, rfTransmitEnabled = false, supportedModes = new[] { "pulsar", "tone", "noise", "am" } }));

app.MapPost("/algorithms/dispersion-delay", (DispersionRequest request) =>
{
    try
    {
        var delay = SignalAlgorithms.DispersionDelayMilliseconds(request.DispersionMeasure, request.LowFrequencyMHz, request.HighFrequencyMHz);
        return Results.Ok(new { delayMilliseconds = delay });
    }
    catch (ArgumentOutOfRangeException exception) { return Results.BadRequest(new { error = exception.Message }); }
});

app.MapPost("/algorithms/fft-magnitude", (FftRequest request) =>
{
    if (request.Samples is null) return Results.BadRequest(new { error = "Samples are required." });
    if (request.Samples.Length > 65_536) return Results.BadRequest(new { error = "FFT input is limited to 65,536 samples." });
    try { return Results.Ok(new { magnitude = SignalAlgorithms.FftMagnitude(request.Samples) }); }
    catch (ArgumentException exception) { return Results.BadRequest(new { error = exception.Message }); }
});

app.MapPost("/channels/plan", (ChannelPlanRequest request) =>
{
    try
    {
        var channels = ChannelPlanner.Create(request.StartFrequencyMHz, request.TotalBandwidthMHz, request.Count, request.LabelPrefix);
        return Results.Ok(new { count = channels.Count, startFrequencyMHz = request.StartFrequencyMHz, totalBandwidthMHz = request.TotalBandwidthMHz, channels });
    }
    catch (ArgumentException exception) { return Results.BadRequest(new { error = exception.Message }); }
});

app.MapPost("/simulation/samples", (SimulationRequest request) =>
{
    if (!double.IsFinite(request.SampleRateHz) || request.SampleRateHz < 100 || request.SampleRateHz > 96_000)
        return Results.BadRequest(new { error = "Sample rate must be between 100 and 96,000 Hz." });
    if (request.DurationSeconds is < 1 or > 10)
        return Results.BadRequest(new { error = "API sample generation duration must be between 1 and 10 seconds." });
    try
    {
        var samples = new PulsarDatasetSimulator().Generate(new PulsarSimulationOptions(
            request.SampleRateHz, request.PulseRateHz, request.ToneFrequencyHz, 0.02, 0.12, request.DurationSeconds, request.Seed));
        return Results.Ok(new { simulatedOnly = true, sampleRateHz = request.SampleRateHz, sampleCount = samples.Length, durationSeconds = request.DurationSeconds, seed = request.Seed, samples });
    }
    catch (ArgumentException exception) { return Results.BadRequest(new { error = exception.Message }); }
});

app.Run();

public sealed record DispersionRequest(double DispersionMeasure, double LowFrequencyMHz, double HighFrequencyMHz);
public sealed record FftRequest(double[]? Samples);
public sealed record ChannelPlanRequest(double StartFrequencyMHz, double TotalBandwidthMHz, int Count, string? LabelPrefix = null);
public sealed record SimulationRequest(double SampleRateHz = 22050, double PulseRateHz = 2, double ToneFrequencyHz = 700, int DurationSeconds = 2, int Seed = 12345);
