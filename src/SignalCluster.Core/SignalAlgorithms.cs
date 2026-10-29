namespace SignalCluster.Core;

public static class SignalAlgorithms
{
    public static double[] FftMagnitude(double[] input)
    {
        ArgumentNullException.ThrowIfNull(input);
        if (input.Length == 0 || (input.Length & (input.Length - 1)) != 0)
            throw new ArgumentException("FFT length must be a non-zero power of two.", nameof(input));
        if (input.Any(value => !double.IsFinite(value)))
            throw new ArgumentException("FFT samples must all be finite numbers.", nameof(input));

        int n = input.Length;
        var re = (double[])input.Clone();
        var im = new double[n];
        for (int i = 1, j = 0; i < n; i++)
        {
            int bit = n >> 1;
            for (; (j & bit) != 0; bit >>= 1) j ^= bit;
            j ^= bit;
            if (i < j)
            {
                (re[i], re[j]) = (re[j], re[i]);
                (im[i], im[j]) = (im[j], im[i]);
            }
        }

        for (int len = 2; len <= n; len <<= 1)
        {
            double angle = -2 * Math.PI / len;
            double w0 = Math.Cos(angle), w1 = Math.Sin(angle);
            for (int start = 0; start < n; start += len)
            {
                double wr = 1, wi = 0;
                for (int j = 0; j < len / 2; j++)
                {
                    int u = start + j, v = u + len / 2;
                    double tr = wr * re[v] - wi * im[v];
                    double ti = wr * im[v] + wi * re[v];
                    re[v] = re[u] - tr; im[v] = im[u] - ti;
                    re[u] += tr; im[u] += ti;
                    (wr, wi) = (wr * w0 - wi * w1, wr * w1 + wi * w0);
                }
            }
            if (len == n) break; // Avoid shifting overflow at the largest supported power of two.
        }

        return Enumerable.Range(0, n).Select(i => Math.Sqrt(re[i] * re[i] + im[i] * im[i])).ToArray();
    }

    // With frequencies in MHz, the cold-plasma dispersion constant yields milliseconds.
    public static double DispersionDelayMilliseconds(double dm, double lowMHz, double highMHz)
    {
        if (!double.IsFinite(dm) || dm < 0) throw new ArgumentOutOfRangeException(nameof(dm));
        if (!double.IsFinite(lowMHz) || lowMHz <= 0) throw new ArgumentOutOfRangeException(nameof(lowMHz));
        if (!double.IsFinite(highMHz) || highMHz <= 0) throw new ArgumentOutOfRangeException(nameof(highMHz));
        double lo = Math.Min(lowMHz, highMHz), hi = Math.Max(lowMHz, highMHz);
        return 4.148808e6 * dm * (1 / (lo * lo) - 1 / (hi * hi));
    }

    public static T[] MergeSort<T>(IEnumerable<T> values, IComparer<T>? comparer = null)
    {
        ArgumentNullException.ThrowIfNull(values);
        comparer ??= Comparer<T>.Default;
        T[] a = values.ToArray(), tmp = new T[a.Length];
        void Sort(int l, int r)
        {
            if (r - l < 2) return;
            int m = l + (r - l) / 2;
            Sort(l, m); Sort(m, r);
            int i = l, j = m, k = l;
            while (i < m && j < r) tmp[k++] = comparer.Compare(a[i], a[j]) <= 0 ? a[i++] : a[j++];
            while (i < m) tmp[k++] = a[i++];
            while (j < r) tmp[k++] = a[j++];
            Array.Copy(tmp, l, a, l, r - l);
        }
        Sort(0, a.Length);
        return a;
    }

    public static int GreatestCommonDivisor(int a, int b)
    {
        long x = Math.Abs((long)a), y = Math.Abs((long)b);
        while (y != 0) (x, y) = (y, x % y);
        if (x > int.MaxValue) throw new OverflowException("GCD result cannot be represented as Int32.");
        return (int)x;
    }
}
