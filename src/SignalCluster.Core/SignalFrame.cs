namespace SignalCluster.Core;
public sealed record SignalFrame(DateTimeOffset TimestampUtc, double SampleRateHz, double CenterFrequencyHz, IReadOnlyList<double> Samples)
{
    public int SampleCount => Samples.Count;
    public double DurationSeconds => SampleRateHz > 0 ? SampleCount / SampleRateHz : 0;
    public void Validate()
    {
        if (SampleRateHz <= 0 || double.IsNaN(SampleRateHz) || double.IsInfinity(SampleRateHz)) throw new ArgumentOutOfRangeException(nameof(SampleRateHz));
        if (CenterFrequencyHz < 0 || double.IsNaN(CenterFrequencyHz) || double.IsInfinity(CenterFrequencyHz)) throw new ArgumentOutOfRangeException(nameof(CenterFrequencyHz));
        if (Samples is null) throw new ArgumentNullException(nameof(Samples));
        if (Samples.Any(x => double.IsNaN(x) || double.IsInfinity(x))) throw new ArgumentException("Samples must be finite.");
    }
}