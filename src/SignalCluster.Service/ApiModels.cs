namespace SignalCluster.Service;
public sealed record ChannelPlanRequest(double StartFrequencyMHz,double TotalBandwidthMHz,int Count,string? LabelPrefix=null);
public sealed record SimulationRequest(double SampleRateHz=22050,double PulseRateHz=2,double ToneFrequencyHz=700,int DurationSeconds=8,int Seed=12345);
public sealed record DeviceAttachRequest(string Id,string Name,string Kind);
