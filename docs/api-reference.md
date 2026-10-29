# API reference

Base URL depends on ASP.NET Core launch configuration. The UI defaults to `http://localhost:5080`; change that constant in `ui/app.js` if the service listens on another address.

- `GET /` — service identity and simulation mode.
- `GET /health` — process health timestamp.
- `GET /build-info` — build metadata.
- `GET /radio/status` — virtual radio state; no RF input and no transmission.
- `POST /algorithms/fft-magnitude` — JSON body `{"samples":[1,1,1,1]}`.
- `POST /algorithms/dispersion-delay` — JSON body `{"dispersionMeasure":10,"lowFrequencyMHz":400,"highFrequencyMHz":800}`.

This API is a development prototype. Configure authentication, authorization, restrictive CORS, and network binding before exposing it beyond localhost.
