using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cooley_Tukey_FFT
{
    internal class FFTOutput
    {
        public double Frequency { get; set; }
        public double Magnitude { get; set; }
        public double Phase { get; set; }
        public double Real { get; set; }
        public double Imaginary { get; set; }

        public FFTOutput(double frequency, double magnitude, double phase, double real, double imaginary)
        {
            Frequency = frequency;
            Magnitude = magnitude;
            Phase = phase;
            Real = real;
            Imaginary = imaginary;
        }

        // Creates a string representation in the form: Frequency Magnitude Phase Real Imaginary
        public override string ToString()
        {
            return $"{Frequency} {Magnitude} {Phase} {Real} {Imaginary}";
        }
    }
}
