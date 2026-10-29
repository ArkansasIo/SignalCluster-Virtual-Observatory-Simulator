namespace SignalCluster.Core;
public enum VirtualDeviceKind { VirtualDisk, ComputeNode, VirtualSdr }
public sealed record VirtualDevice(string Id, string Name, VirtualDeviceKind Kind, string State, bool IsSimulated = true);
public interface IVirtualDevicePlugin
{
    string Id { get; }
    string DisplayName { get; }
    IReadOnlyList<VirtualDevice> EnumerateDevices();
}
public sealed class InMemoryDevicePlugin : IVirtualDevicePlugin
{
    private readonly List<VirtualDevice> _devices = new();
    public string Id => "signalcluster.virtual";
    public string DisplayName => "SignalCluster In-Memory Device Provider";
    public IReadOnlyList<VirtualDevice> EnumerateDevices() => _devices.AsReadOnly();
    public void Add(VirtualDevice device)
    {
        if (string.IsNullOrWhiteSpace(device.Id)) throw new ArgumentException("Device ID required.");
        if (_devices.Any(d=>d.Id==device.Id)) throw new InvalidOperationException($"Device '{device.Id}' already exists.");
        _devices.Add(device with { IsSimulated = true });
    }
}