# SignalCluster — Virtual Observatory Simulator

**Developer:** ArkansasIo  
**Version:** 0.2.0 (prototype)  
**Build ID:** `SC-SIM-2026.10.29-001`  
**Target:** .NET 8 / Windows development environment

SignalCluster is a software-first virtual radio astronomy and signal-processing workbench. The prototype is intended to support synthetic signal generation, channel planning, algorithm experiments, logical cluster/device configuration, and a browser-based control interface.

## Current scope

- Browser UI for cluster overview, channel planning, algorithm workbench, pulsar simulation, storage inventory, and build information.
- Synthetic pulsar/audio simulation; generated audio is not a recording of an astronomical source.
- Virtual-device plugin abstractions for future device adapters.
- ASP.NET Core control API intended for local development.
- Radio/audio monitor concept: `SignalCluster Virtual Radio Receiver (SC-VRR-01)` is virtual only. No physical SDR or RF transmission is implemented.

## Safety and prototype limitations

This is a prototype. Logical drive slots are not physical disk discovery or mounting. The simulator does not receive live RF by itself, and it does not control or transmit through physical radio hardware. Do not expose the development API to an untrusted network without configuring authentication, authorization, and restrictive CORS.

## Build and run

Requires the .NET 8 SDK. From the repository root, once the complete source tree is present:

```sh
dotnet restore SignalCluster.sln
dotnet build SignalCluster.sln --configuration Release
dotnet test SignalCluster.sln --configuration Release
```

The browser UI is in `ui/`. Consult `docs/` for algorithm, versioning, and virtual-radio notes when those files are present.

## Verification status

The prototype source bundle was syntax-checked for JavaScript and its JSON files were parsed. A .NET build/test has not been verified in this environment. Treat build status as **unverified** until CI completes successfully.
