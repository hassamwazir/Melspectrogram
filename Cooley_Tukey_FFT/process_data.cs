using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cooley_Tukey_FFT
{
    public class process_data
    {
        // Store input data
        // int sample_count;
        List<ComplexNumber> inpt_sample = new List<ComplexNumber>();

        // Output data
        List<double> outpt_freq = new List<double>();
        List<ComplexNumber> outpt_signal = new List<ComplexNumber>();

        public List<double> freq_output
        {
            get { return outpt_freq; }
        }

        public List<ComplexNumber> fft_output
        {
            get { return outpt_signal; }
        }

        public process_data(List<double> i_time, List<double> i_amplitude)
        {
            // Time domain to Frequency domain
            // Number of samples
            int i, sample_size, sample_count;
            sample_count = i_amplitude.Count;
            sample_size = i_amplitude.Count;

            for (i = 0; i < sample_count; i++)
            {
                // Change imaginary value from 0 to solve complex signal
                inpt_sample.Add(new ComplexNumber(i_amplitude[i], 0));
            }

            while (IsPowerOfTwo(sample_count) == false)
            {
                sample_count++;
                // Add zero at end to make it power of 2
                // As the FFT operates on inputs that contain an integer power of two number of samples, 
                // the input data length will be augmented by zero padding at the end.
                inpt_sample.Add(new ComplexNumber(0, 0));
            }

            // Fast fourier transform
            int FFT_output_length;
            List<ComplexNumber> outpt_signal_shift = new List<ComplexNumber>();
            outpt_signal_shift = FFT(inpt_sample);
            FFT_output_length = outpt_signal_shift.Count;

            // FFTshift shifts the DC component to the center of the spectrum. 
            // It is important to remember that the Nyquist frequency at the (N/2+1)th index is common to both positive and negative frequency sides. 
            // FFTshift command puts the Nyquist frequency in the negative frequency side.
            outpt_signal = new List<ComplexNumber>();

            for (i = (FFT_output_length / 2); i < FFT_output_length; i++)
            {
                outpt_signal.Add(outpt_signal_shift[i]);
            }

            for (i = 0; i < (FFT_output_length / 2); i++)
            {
                outpt_signal.Add(outpt_signal_shift[i]);
            }
            // ____

            double sample_time = i_time[i_time.Count - 1] - i_time[0];
            double frequency_resoultion = ((double)sample_size / (double)(sample_time * FFT_output_length));
            outpt_freq = new List<double>();

            for (i = -(FFT_output_length / 2); i < (FFT_output_length / 2) + 1; i++)
            {
                outpt_freq.Add(i * frequency_resoultion);
            }
        }

        // The Fast Fourier Transform (FFT) algorithm
        public static List<ComplexNumber> FFT(List<ComplexNumber> inpt_signal)
        {
            int i;
            int N = inpt_signal.Count;
            if (N == 1) // Base case. Return the input signal as the output signal (since the FFT of a single value is itself)
                return inpt_signal;

            // Even array
            List<ComplexNumber> evenList = new List<ComplexNumber>();
            for (i = 0; i < (N / 2); i++)
            {
                evenList.Add(inpt_signal[2 * i]);
            }
            evenList = FFT(evenList);

            // Odd array
            List<ComplexNumber> oddList = new List<ComplexNumber>();
            for (i = 0; i < (N / 2); i++)
            {
                oddList.Add(inpt_signal[(2 * i) + 1]);
            }
            oddList = FFT(oddList);

            // Result
            ComplexNumber[] result = new ComplexNumber[N];

            for (i = 0; i < (N / 2); i++)
            {
                double w = (-2.0 * i * Math.PI) / N;
                ComplexNumber wk = new ComplexNumber(Math.Cos(w), Math.Sin(w));
                ComplexNumber even = evenList[i];
                ComplexNumber odd = oddList[i];

                result[i] = even + (wk * odd);
                result[i + (N / 2)] = even - (wk * odd);
            }
            return result.ToList();
        }

        public static bool IsPowerOfTwo(int n)
        {
            return (n != 0) && (n & (n - 1)) == 0;
        }

        public static double[] HammingWindow(int windowLength)
        {
            double[] window = new double[windowLength];
            for (int i = 0; i < windowLength; i++)
            {
                window[i] = 0.54 - 0.46 * Math.Cos((2 * Math.PI * i) / (windowLength - 1));
            }
            return window;
        }
        public static double[] HanningWindow(int windowLength)
        {
            double[] window = new double[windowLength];
            for (int i = 0; i < windowLength; i++)
            {
                window[i] = 0.5 - 0.5 * Math.Cos((2 * Math.PI * i) / (windowLength - 1));
            }
            return window;
        }

        public List<List<double>> GenerateSpectrogram(int n_fft, int hop_length, int win_length)
        {
            int numSegments = (inpt_sample.Count - win_length) / hop_length + 1;
            List<List<double>> spectrogram = new List<List<double>>();

            double[] window = HanningWindow(win_length);

            // Process each segment
            for (int start = 0; start + win_length <= inpt_sample.Count; start += hop_length)
            {
                List<ComplexNumber> segment = new List<ComplexNumber>();

                // Apply window function and optionally pad segment to match n_fft
                for (int i = 0; i < win_length; i++)
                {
                    double windowedValue = inpt_sample[start + i].Real * window[i];
                    segment.Add(new ComplexNumber(windowedValue, 0));
                }

                // If segmentSize is less than n_fft, pad the segment with zeros up to n_fft
                if (win_length < n_fft)
                {
                    int paddingSize = n_fft - win_length;
                    for (int i = 0; i < paddingSize; i++)
                    {
                        segment.Add(new ComplexNumber(0, 0));
                    }
                }

                // Compute FFT of the windowed (and possibly padded) segment
                List<ComplexNumber> fftResult = FFT(segment);
                // Calculate power spectrum from FFT result
                List<double> powerSpectrum = fftResult.Select(c => c.MagnitudeSquared()).ToList();

                // Optionally, consider only the first (nFft/2)+1 bins if onesided is true
                spectrogram.Add(powerSpectrum.GetRange(0, (n_fft / 2) + 1));
            }

            return spectrogram;
        }
    }
}
