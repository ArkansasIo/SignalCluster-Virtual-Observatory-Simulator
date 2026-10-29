using SignalCluster.Core;
using Xunit;

public class ChannelPlannerTests
{
    [Fact]
    public void CreatesEvenlySpacedChannels()
    {
        var channels = ChannelPlanner.Create(1450, 100, 4, "TEST");
        Assert.Equal(4, channels.Count);
        Assert.Equal(1400, channels[0].LowerEdgeMHz, 8);
        Assert.Equal(1500, channels[^1].UpperEdgeMHz, 8);
        Assert.Equal(25, channels[0].BandwidthMHz, 8);
        Assert.Equal("TEST-0001", channels[0].Label);
    }

    [Theory]
    [InlineData(0, 100, 4)]
    [InlineData(1450, 0, 4)]
    [InlineData(1450, 100, 0)]
    [InlineData(1450, 100, 16385)]
    public void RejectsInvalidPlans(double start, double bandwidth, int count)
    {
        Assert.ThrowsAny<ArgumentException>(() => ChannelPlanner.Create(start, bandwidth, count));
    }
}
