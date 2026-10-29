namespace SignalCluster.Core;
public sealed record ChannelSpec(int Index, double CenterFrequencyMHz, double BandwidthMHz, string Label)
{
    public double LowerEdgeMHz => CenterFrequencyMHz - BandwidthMHz / 2;
    public double UpperEdgeMHz => CenterFrequencyMHz + BandwidthMHz / 2;
    public void Validate()
    {
        if (Index < 0) throw new ArgumentOutOfRangeException(nameof(Index));
        if (!double.IsFinite(CenterFrequencyMHz) || CenterFrequencyMHz <= 0) throw new ArgumentOutOfRangeException(nameof(CenterFrequencyMHz));
        if (!double.IsFinite(BandwidthMHz) || BandwidthMHz <= 0 || BandwidthMHz >= 2 * CenterFrequencyMHz) throw new ArgumentOutOfRangeException(nameof(BandwidthMHz));
        if (string.IsNullOrWhiteSpace(Label)) throw new ArgumentException("A channel label is required.", nameof(Label));
    }
}
public static class ChannelPlanner
{
    public const int MaxPlanningChannels = 16384;
    public static IReadOnlyList<ChannelSpec> CreateUniform(double startMHz, double totalBandwidthMHz, int count, string prefix = "CH")
    {
        if (!double.IsFinite(startMHz) || startMHz <= 0) throw new ArgumentOutOfRangeException(nameof(startMHz));
        if (!double.IsFinite(totalBandwidthMHz) || totalBandwidthMHz <= 0) throw new ArgumentOutOfRangeException(nameof(totalBandwidthMHz));
        if (count < 1 || count > MaxPlanningChannels) throw new ArgumentOutOfRangeException(nameof(count), $"Count must be between 1 and {MaxPlanningChannels}.");
        double width = totalBandwidthMHz / count;
        return Enumerable.Range(0,count).Select(i => new ChannelSpec(i,startMHz + width*(i+0.5),width,$"{prefix}-{i+1:D4}")).ToArray();
    }
}