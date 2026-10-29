# Channel planning

`ChannelPlanner.CreateUniform(startMHz, totalBandwidthMHz, count)` produces evenly spaced channel centers and bandwidths. The software planning limit is 16,384 channels. This is a metadata-generation bound, not a throughput guarantee, filter-bank implementation, or performance claim. Verify frequency range, Nyquist constraints, calibration, and instrument bandwidth before using any plan with real samples.
