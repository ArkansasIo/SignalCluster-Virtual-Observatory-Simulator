# Virtual device plugin architecture

The current provider is deliberately in-memory and simulation-only. Device plugins implement `IVirtualDevicePlugin` and expose device metadata without performing physical disk operations or radio control. A future SDR adapter should report driver, model, supported sample rates, frequency range, channel count, and receive-only/transmit capabilities explicitly. Real hardware integration must be opt-in, permission-aware, and tested on the target OS.

Logical drive inventory is planning metadata, not a disk scan, mount operation, or storage benchmark.
