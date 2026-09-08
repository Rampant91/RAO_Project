using System;
using System.Globalization;

namespace Models.Comparers.FormContent;

public static class FormDoubleEquality
{
    public const double DefaultAbsoluteTolerance = 1e-9;
    public const double DefaultRelativeTolerance = 1e-6;

    public static bool Equals(double a, double b, double absoluteTolerance = DefaultAbsoluteTolerance,
        double relativeTolerance = DefaultRelativeTolerance)
    {
        if (double.IsNaN(a) || double.IsNaN(b))
        {
            return double.IsNaN(a) && double.IsNaN(b);
        }

        if (a == b)
        {
            return true;
        }

        if (double.IsInfinity(a) || double.IsInfinity(b))
        {
            return false;
        }

        var diff = Math.Abs(a - b);
        var scale = Math.Max(Math.Abs(a), Math.Abs(b));
        return diff <= absoluteTolerance || diff <= relativeTolerance * scale;
    }

    public static bool Equals(float? a, float? b, float absoluteTolerance = 1e-5f)
    {
        if (!a.HasValue && !b.HasValue)
        {
            return true;
        }

        if (!a.HasValue || !b.HasValue)
        {
            return false;
        }

        return Equals(a.Value, b.Value, absoluteTolerance);
    }

    public static bool Equals(float a, float b, float absoluteTolerance = 1e-5f)
    {
        if (float.IsNaN(a) || float.IsNaN(b))
        {
            return float.IsNaN(a) && float.IsNaN(b);
        }

        if (a == b)
        {
            return true;
        }

        if (float.IsInfinity(a) || float.IsInfinity(b))
        {
            return false;
        }

        return MathF.Abs(a - b) <= absoluteTolerance;
    }

    /// <summary>
    /// Канонизация double/float для fingerprint: округление согласовано с допуском Equals.
    /// </summary>
    public static string NormalizeForFingerprint(double value)
    {
        if (double.IsNaN(value))
        {
            return "NaN";
        }

        if (double.IsPositiveInfinity(value))
        {
            return "+Inf";
        }

        if (double.IsNegativeInfinity(value))
        {
            return "-Inf";
        }

        // 1e-9 abs tolerance → 9 decimal places is enough for stable equality buckets
        var rounded = Math.Round(value, 9, MidpointRounding.AwayFromZero);
        return rounded.ToString("G17", CultureInfo.InvariantCulture);
    }

    public static string NormalizeForFingerprint(float value) =>
        NormalizeForFingerprint((double)value);
}
