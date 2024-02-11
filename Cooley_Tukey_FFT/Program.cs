using System;
using System.Text;
using System.Xml.Linq;

namespace Cooley_Tukey_FFT
{
    static class Program
    {
        static void Main(string[] args)
        {
            // Specify the current directory path
            string currentDirectoryPath = Directory.GetCurrentDirectory();
            Console.WriteLine("The current directory is {0}", currentDirectoryPath);

            Console.WriteLine("Loading Data");

            // create data series
            List<double> x_datas = new List<double>();
            List<double> y_datas = new List<double>();

            // create FFT outputs
            List<FFTOutput> fftOutputs = new List<FFTOutput>();

            // read a data file called "data.txt" and store in x_datas and y_datas
            string[] lines = File.ReadAllLines(@"../../../../cosine_example.txt");

            // print out the data to the console

            foreach (string line in lines)
            {
                //Console.WriteLine(line);
                string[] words = line.Split('\t');
                //Console.WriteLine(words[0] + ' ' + words[1]);
                x_datas.Add(Convert.ToDouble(words[0]));
                y_datas.Add(Convert.ToDouble(words[1]));
            }

            Console.WriteLine("Data Loaded");

            process_data pd = new process_data(x_datas, y_datas);

            for (int i = 0; i < pd.fft_output.Count; i++)
            {
                // Add the frequency, magnitude, phase, real and imaginary parts of the FFT output to an array
                //Console.WriteLine("{0} {1} {2} {3} {4}\n", pd.freq_output[i].ToString(),
                //                                         pd.fft_output[i].GetMagnitude().ToString(),
                //                                         pd.fft_output[i].GetPhase().ToString(),
                //                                         pd.fft_output[i].Real.ToString(),
                //                                         pd.fft_output[i].Imag.ToString());

                FFTOutput output = new FFTOutput(pd.freq_output[i], 
                                                pd.fft_output[i].GetMagnitude(), 
                                                pd.fft_output[i].GetPhase(), 
                                                pd.fft_output[i].Real, 
                                                pd.fft_output[i].Imag);


                //(pd.freq_output[i].ToString(),
                //                                      pd.fft_output[i].GetMagnitude().ToString(),
                //                                      pd.fft_output[i].GetPhase().ToString(),
                //                                      pd.fft_output[i].Real.ToString(),
                //                                      pd.fft_output[i].Imag.ToString());
            }

        }
    }
}