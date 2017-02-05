using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using PerlinNoiseApp;
using System.Collections.Generic;

namespace PerlinNoiseAppUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Noise_CreatePermutationArray_CorrectLength()
        {
            var t = Enumerable.Range(0, 256).ToArray();
            Assert.AreEqual(256,t.Length);
        }
        [TestMethod]
        public void Noise_ShufflePermutationArray_Valid()
        {
            var temp = Enumerable.Range(0, 256).ToArray();
            Noise noise = new Noise();
            noise.Shuffle(temp);
        }
        [TestMethod]
        public void Noise_ShufflePermutationArray_IsShuffled()
        {
            var before = Enumerable.Range(0, 256).ToArray();
            Noise noise = new Noise();
            var after = new int[256];
            for(int i = 0; i < 256 ;i++)
            {
                after[i] = before[i];
            }
            noise.Shuffle(after);
            Assert.IsFalse(before.SequenceEqual(after));

        }

        [TestMethod]
        public void Noise_CreateGradientList_Valid()
        {
            Random _random = new Random();
            List<Tuple<double, double>> _gradient;
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
        }

    }
}
