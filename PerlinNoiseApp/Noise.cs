using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerlinNoiseApp
{
    public class Noise
    {
        public static int[] _permutation;
        private static List<Tuple<double, double>> _gradient; //Using Tuple to indicate point X and Y
        private static Random _random = new Random();
        public Noise()
        {
            //Randomly generate permutation array from 0 to 255
            _permutation = Enumerable.Range(0, 256).ToArray();
            Shuffle(_permutation);
            #region Create Gradient Array
            _gradient = new List<Tuple<double, double>>();

            double distance = 0;

            for (int i = 0; i < 256; i++)
            {
                Tuple<double, double> gradient;
                _gradient.Add(null);
                do
                {
                    gradient = new Tuple<double, double>(_random.NextDouble() * 2 - 1, _random.NextDouble() * 2 - 1); //randomly create points
                    distance = Math.Sqrt((gradient.Item1 * gradient.Item1) + (gradient.Item2 * gradient.Item2)); //find euclidean distance
                } while (distance >= 1);

                //Normalizing vector by dividing its points with the euclidean distance
                gradient = new Tuple<double, double>(gradient.Item1 / distance, gradient.Item2 / distance);
                _gradient[i] = gradient;
            } 
            #endregion

        }
        public double CreateNoise(double x, double y)
        {
            var cell = new Tuple<double, double>(Math.Floor(x), Math.Floor(y));
            var sum = 0.0;
            var corners = new List<Tuple<double, double>>() { new Tuple<double, double>(0, 0), new Tuple<double, double>(0, 1), new Tuple<double, double>(1, 0), new Tuple<double, double>(1, 1) };
            foreach (var corner in corners)
            {
                var i = cell.Item1 + corner.Item1;
                var j = cell.Item2 + corner.Item2;
                var u = x - i;
                var v = y - j;
                var index = _permutation[Convert.ToInt32(i)];
                index = _permutation[(index + Convert.ToInt32(j)) % _permutation.Length];
                var grad = _gradient[index % _gradient.Count];
                sum += Q(u, v) * Dot(grad, new Tuple<double, double>(u, v));
            }
           
            return Math.Max(Math.Min(sum, 1.0), -1.0);
        }
        public void Shuffle(int[] array)
        {
            //Shuffle array items
            int index = array.Length;
            while (index > 1)
            {
                int k = _random.Next(index--);
                var temp = array[index];
                array[index] = array[k];
                array[k] = temp;
            }
        }


        public void DrawImage(int width, int height)
        {
            var scale = 1.2;//More noise get generated if scale is low.
            Bitmap image = new Bitmap(width, height);
            //iterate every pixel in the image;
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    var pixel = Convert.ToInt32(CreateNoise(x / scale, y / scale) + 1) * 128;                    
                    Byte[] values = BitConverter.GetBytes(pixel);
                    image.SetPixel(x, y, Color.FromArgb(values[0],values[1],values[2]));

                }
            }
            //Save generated image to directory
            image.Save(@"E:\perlinnoise.jpg");
        }
        public double Drop(double t)
        {
            t = Math.Abs(t);
            return 1.0 - t * t * t * (t * (t * 6 - 15) + 10); //Improved Perlin Noise Formula
        }
        public double Q(double u, double v)
        {
            return Drop(u) * Drop(v); //Fading
        }
        public double Dot(Tuple<double, double> tuple1, Tuple<double, double> tuple2)
        {
            return (tuple1.Item1 * tuple2.Item1) + (tuple1.Item2 * tuple2.Item2);
        }

    }
}
