namespace SignalCluster.Core;
public sealed record PulsarSimulationOptions(double SampleRateHz=22050, double PulseRateHz=2, double ToneFrequencyHz=700, double NoiseAmplitude=0.02, double PulseWidthFraction=0.12, int DurationSeconds=8, int Seed=12345);
public sealed class PulsarDatasetSimulator
{
    public float[] Generate(PulsarSimulationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        if (options.SampleRateHz < 100 || options.PulseRateHz <= 0 || options.ToneFrequencyHz <= 0 || options.NoiseAmplitude < 0 || options.PulseWidthFraction <= 0 || options.PulseWidthFraction >= 1 || options.DurationSeconds is < 1 or > 120)
            throw new ArgumentOutOfRangeException(nameof(options), "Simulation parameters are outside supported limits.");
        int count=checked((int)(options.SampleRateHz*options.DurationSeconds));
        var output=new float[count]; var random=new Random(options.Seed);
        for(int i=0;i<count;i++)
        {
            double t=i/options.SampleRateHz, phase=(t*options.PulseRateHz)%1;
            double pulse=phase<options.PulseWidthFraction ? Math.Sin(Math.PI*phase/options.PulseWidthFraction) : 0;
            double value=0.8*pulse*Math.Sin(2*Math.PI*options.ToneFrequencyHz*t)+(random.NextDouble()*2-1)*options.NoiseAmplitude;
            output[i]=(float)Math.Clamp(value,-1,1);
        }
        return output;
    }
}