using System;
using System.Text;
using System.Xml.Linq;

namespace Cooley_Tukey_FFT
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Loading Data");

            // create data series
            List<double> x_datas = new List<double>();
            List<double> y_datas = new List<double>();

            // read a data file called "data.txt" and store in x_datas and y_datas
            string[] lines = System.IO.File.ReadAllLines(@"../data.txt");
            foreach (string line in lines)
            {
                string[] words = line.Split(' ');
                x_datas.Add(Convert.ToDouble(words[0]));
                y_datas.Add(Convert.ToDouble(words[1]));
            }



            process_data pd = new process_data(x_datas, y_datas);

        }
    }
}