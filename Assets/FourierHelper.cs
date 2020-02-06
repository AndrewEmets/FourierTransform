using System.Collections.Generic;
using UnityEngine;

public static class FourierHelper
{
    public struct FourierData
    {
        public readonly float amplitude;
        public readonly float frequency;
        public readonly float phase;

        public FourierData(float amplitude, float frequency, float phase)
        {
            this.amplitude = amplitude;
            this.frequency = frequency;
            this.phase = phase;
        }
    }
    
    public static List<FourierData> DFT(List<ComplexNumber> points)
    {
        var result = new List<FourierData>(points.Count);

        var N = points.Count;
        for (var k = 0; k < N; k++)
        {
            ComplexNumber xk = default;
        
            for (var n = 0; n < N; n++)
            {
                var f = 2*Mathf.PI * (k - N/2) * n / N;
                var e = new ComplexNumber(Mathf.Cos(f), -Mathf.Sin(f));

                xk += points[n] * e;
            }

            xk /= N;

            var freq = k - points.Count/2;
            var amp = ((Vector2) xk).magnitude;
            var phase = Mathf.Atan2(xk.b, xk.a);
            
            result.Add(new FourierData(amp, freq, phase));
        }

        return result;
    }
}