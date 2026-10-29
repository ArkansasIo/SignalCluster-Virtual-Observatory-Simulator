# SignalCluster — Virtual Observatory Simulator

**Developer:** ArkansasIo  
**Version:** 0.2.0 (prototype)  
**Build ID:** `SC-SIM-2026.10.29-001`  
**Runtime:** .NET 8 / ASP.NET Core with a browser-based UI

SignalCluster is a software-only educational prototype for synthetic radio astronomy signals and signal-processing experiments. It does not connect to real radio-frequency hardware or a telescope. The storage inventory and receiver are virtual models.

## Prototype features

- ASP.NET Core status and build-info endpoints.
- FFT magnitude and cold-plasma dispersion-delay calculations.
- Validated channel-plan generation (up to 16,384 metadata channels).
- Bounded, seeded synthetic pulsar-like sample generation.
- Browser-based synthetic audio modes and WAV export.
- Example channel-plan and logical storage inventory CSV files.
- .NET unit tests and GitHub Actions build workflow.

## Requirements

- .NET 8 SDK
- Python 3 (only to serve the static UI locally), or another static file server
- A modern browser with Web Audio API support for audio playback

## Run locally

From the repository root, start the API:

```powershell
dotnet restore .\SignalCluster.sln
dotnet build .\SignalCluster.sln -c Release
dotnet test .\SignalCluster.sln -c Release
dotnet run --project .\src\SignalCluster.Service\SignalCluster.Service.csproj
```

The launch profile configures the API at `http://localhost:5080`. In a second terminal, serve the UI:

```powershell
python -m http.server 5173 --directory ui
```

Open `http://localhost:5173`. API documentation and request examples are in [docs/api-reference.md](docs/api-reference.md).

## API overview

- `GET /health`
- `GET /build-info`
- `GET /radio/status`
- `POST /algorithms/dispersion-delay`
- `POST /algorithms/fft-magnitude`
- `POST /channels/plan`
- `POST /simulation/samples`

## Prototype boundaries

- Generated samples are synthetic and do not represent a real astronomical observation.
- No live SDR, RF transmission, telescope control, or hardware discovery is implemented.
- The 24-drive inventory is logical planning metadata, not physical disks.
- Lua configuration is an example; sandboxed runtime execution is not implemented.
- Build/test status must be determined from actual local or GitHub Actions output. Do not assume a successful build merely because files are committed.
