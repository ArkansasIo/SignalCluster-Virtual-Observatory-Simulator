namespace SignalCluster.Core;

public sealed record ChannelPlanEntry(int Index, string Label, double CenterFrequencyMHz, double BandwidthMHz, double LowerEdgeMHz, double UpperEdgeMHz);

public static class ChannelPlanner
{
    public const int MaximumChannels = 16_384;

    public static IReadOnlyList<ChannelPlanEntry> Create(double startFrequencyMHz, double totalBandwidthMHz, int count, string? labelPrefix = null)
    {
        if (!double.IsFinite(startFrequencyMHz) || startFrequencyMHz <= 0)
            throw new ArgumentOutOfRangeException(nameof(startFrequencyMHz), "Start frequency must be finite and positive.");
        if (!double.IsFinite(totalBandwidthMHz) || totalBandwidthMHz <= 0)
            throw new ArgumentOutOfRangeException(nameof(totalBandwidthMHz), "Total bandwidth must be finite and positive.");
        if (count is < 1 or > MaximumChannels)
            throw new ArgumentOutOfRangeException(nameof(count), $"Channel count must be between 1 and {MaximumChannels}.");
        if (startFrequencyMHz - totalBandwidthMHz / 2 <= 0)
            throw new ArgumentOutOfRangeException(nameof(startFrequencyMHz), "The requested band would extend to zero or negative frequency.");
        if (string.IsNullOrWhiteSpace(labelPrefix)) labelPrefix = "CH";
        labelPrefix = labelPrefix.Trim();
        if (labelPrefix.Length > 32) throw new ArgumentException("Label prefix cannot exceed 32 characters.", nameof(labelPrefix));

        double channelBandwidth = totalBandwidthMHz / count;
        var channels = new ChannelPlanEntry[count];
        for (int i = 0; i < count; i++)
        {
            double lower = startFrequencyMHz - totalBandwidthMHz / 2 + i * channelBandwidth;
            double upper = lower + channelBandwidth;
            channels[i] = new ChannelPlanEntry(i, $"{labelPrefix}-{i + 1:D4}", (lower + upper) / 2, channelBandwidth, lower, upper);
        }
        return channels;
    }
}
