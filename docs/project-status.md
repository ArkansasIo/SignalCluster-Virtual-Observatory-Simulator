# Project status — prototype

## Implemented in source
- .NET 8 solution layout with core library, minimal API service, and xUnit test project.
- FFT magnitude, dispersion-delay estimate, merge sort, and recursive GCD utilities.
- Channel metadata planning and synthetic pulsar sample generator.
- Virtual-device interface and in-memory simulated provider.
- 24-slot logical storage inventory and CSV planning examples.
- Browser UI with simulated audio playback and WAV export.
- Build metadata, Lua configuration example, and project documentation.

## Not implemented or not verified
- Live SDR/receiver enumeration and physical RF ingestion.
- Real-time filterbank/channelizer, calibrated astronomy pipeline, or telescope integration.
- Physical disk detection, formatting, mounting, RAID, or storage throughput tests.
- Embedded Lua runtime or sandbox.
- .NET build/test success until the GitHub Actions workflow has completed successfully.

All sample signals and drive records are synthetic planning artifacts. They must not be represented as live hardware measurements.
