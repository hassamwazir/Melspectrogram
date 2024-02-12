using NWaves.FeatureExtractors;
using NWaves.FeatureExtractors.Options;
using NWaves.Filters.Fda;
using NWaves.Transforms;
using NWaves.Utils;
using NWaves.Windows;
using System;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;

namespace Cooley_Tukey_FFT
{
    static class Program
    {
        static void Main(string[] args)
        {
            int n_fft = 2048;
            int hop_length = 64;
            int n_mels = 128;
            int sr = 16000;
            int win_length = n_fft;



            //var filterbank = FilterBanks.Triangular(n_fft, sr, FilterBanks.MelBands(n_mels, sr, 100, 7500), mapper: Scale.HerzToMel);

            var options = new FilterbankOptions
            {
                SamplingRate = sr,
                FrameSize = n_fft,
                FftSize = n_fft,
                HopSize = hop_length,
                Window = WindowType.Hann,
                FilterBank = FilterBanks.Triangular(n_fft, sr, FilterBanks.MelBands(n_mels, sr)),

            };

            var extractor = new FilterbankExtractor(options);

            // Specify the current directory path
            string currentDirectoryPath = Directory.GetCurrentDirectory();
            Console.WriteLine("The current directory is {0}", currentDirectoryPath);

            Console.WriteLine("Loading Data");

            // create data series
            List<float> audio_data = new List<float>();

            // read a data file called "data.txt" and store in x_datas and y_datas
            string[] lines = File.ReadAllLines(@"../../../../chirp.txt");

            // print out the data to the console

            foreach (string line in lines)
            {
                string[] words = line.Split(' ');
                audio_data.Add(Convert.ToSingle(words[0]));
            }

            //var offset = hop_length - (n_fft / 2) % hop_length;

            //Console.WriteLine("Offset: " + offset);

            //add padding to the start and end of audio data
            //for (int i = 0; i < offset; i++)
            //{
            //    audio_data.Insert(0, 0);
            //}

            //for (int i = 0; i < offset; i++)
            //{
            //    audio_data.Add(0);
            //}

            // length of the audio data
            //int audio_data_length = audio_data.Count;
            //Console.WriteLine("Length of Audio Data: " + audio_data_length);

            // create a string to hold the output data
            string output_data = "";

            var vectors = extractor.ComputeFrom(audio_data.ToArray());

            Console.WriteLine("Length of Vectors: " + vectors.Count);

            // save the spectrogram data to a file called "spectrogram.txt"
            //foreach (var row in vectors)
            //{
            //    foreach (var value in row)
            //    {
            //        output_data += value.ToString() + "\t";
            //    }
            //    output_data += "\n";
            //}



            // Initialize a StringBuilder to hold the output data
            StringBuilder outputBuilder = new StringBuilder();

            output_data = "";

            // Determine the number of features (columns) and vectors (rows)
            int numberOfFeatures = vectors[0].Length;  // Assuming all vectors have the same length
            int numberOfVectors = vectors.Count;

            // Iterate through each feature (column in the original data)
            for (int featureIndex = 0; featureIndex < numberOfFeatures; featureIndex++)
            {
                // For each feature, iterate through all vectors (rows in the original data)
                for (int vectorIndex = 0; vectorIndex < numberOfVectors; vectorIndex++)
                {
                    // Append the feature value to the output, followed by a tab character
                    outputBuilder.Append(vectors[vectorIndex][featureIndex].ToString() + "\t");
                }
                // After appending all values for a feature, add a newline to start a new row in the output
                outputBuilder.AppendLine();
            }

            // Convert the StringBuilder content to a string
            output_data = outputBuilder.ToString();

            Console.WriteLine("Data Saved");
            // save the data to a file called "fft_output.txt"
            File.WriteAllText(@"../../../../my_mel_spectrogram.txt", output_data);
        }

    }
}