# API reference

Default local base URL: `http://localhost:5080` (configured by `Properties/launchSettings.json`).

All returned radio samples are synthetic. This service does not connect to an SDR, telescope, or physical RF input.

## Status endpoints

- `GET /` — service name, version, and simulation mode.
- `GET /health` — process health and UTC timestamp.
- `GET /build-info` — build identity and runtime target.
- `GET /radio/status` — virtual receiver capabilities; `rfInputConnected` and `rfTransmitEnabled` remain false.

## Signal algorithms

### `POST /algorithms/dispersion-delay`

Request:

```json
{"dispersionMeasure":10,"lowFrequencyMHz":400,"highFrequencyMHz":800}
```

Returns `delayMilliseconds`. Frequencies are in MHz and the result is in milliseconds. Negative/non-finite dispersion measure or non-positive/non-finite frequencies return HTTP 400.

### `POST /algorithms/fft-magnitude`

Request:

```json
{"samples":[1,1,1,1]}
```

Returns the radix-2 FFT magnitude array. Input must be a non-empty power-of-two length, no more than 65,536 samples, and all values must be finite. Invalid inputs return HTTP 400.

## Channel planning

### `POST /channels/plan`

Request:

```json
{"startFrequencyMHz":1450,"totalBandwidthMHz":100,"count":1024,"labelPrefix":"OBS"}
```

The start frequency is the centre of the requested band. The response includes evenly spaced channels with index, label, centre, bandwidth, lower edge, and upper edge. Count is limited to 16,384. This is channel metadata, not a physical filterbank or real-time tuner.

## Synthetic sample generation

### `POST /simulation/samples`

Request:

```json
{"sampleRateHz":22050,"pulseRateHz":2,"toneFrequencyHz":700,"durationSeconds":2,"seed":12345}
```

Returns a bounded array of deterministic synthetic pulsar-like float samples, sample rate, count, duration, seed, and `simulatedOnly: true`. The API accepts sample rates from 100 to 96,000 Hz and durations from 1 to 10 seconds. A 10-second request at the maximum rate can still return a large JSON response; use modest settings for browser testing.

## Local development

Start the service:

```powershell
dotnet run --project .\src\SignalCluster.Service\SignalCluster.Service.csproj
```

Serve the UI from the repository root in another terminal:

```powershell
python -m http.server 5173 --directory ui
```

Then open `http://localhost:5173`. CORS is restricted to localhost ports 5173. Do not expose this prototype service to untrusted networks.
