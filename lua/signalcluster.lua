-- SignalCluster example configuration (Lua 5.1+)
local SignalCluster = {
  version = "0.2.0",
  mode = "simulation-only",
  radio = { device = "SC-VRR-01", source = "synthetic", transmit = false },
  channels = { start_mhz = 1400.0, bandwidth_mhz = 100.0, count = 1024 },
  storage = { logical_slots = 24, capacity_tb_each = 32 }
}
function SignalCluster.validate()
  assert(SignalCluster.channels.start_mhz > 0, "start_mhz must be positive")
  assert(SignalCluster.channels.bandwidth_mhz > 0, "bandwidth must be positive")
  assert(SignalCluster.channels.count >= 1 and SignalCluster.channels.count <= 16384, "channel count out of range")
  assert(SignalCluster.radio.transmit == false, "prototype does not support RF transmission")
  return true
end
return SignalCluster
