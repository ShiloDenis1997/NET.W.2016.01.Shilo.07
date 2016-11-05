using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Task2.Logic.Tests
{
    [TestFixture]
    public class PolinomeTests
    {
        private double epsilon = Polinome.Epsilon;

        [TestCase(new double[] {5, 0, 0, 12, 15})]
        [TestCase(new double[] {1, 2, 3, 4, 5})]
        [TestCase(new double[] {0, 0, 0})]
        [TestCase(new double[] {0, 0, 2, 3})]
        [Test]
        public void Polinome_Factors_InstanceExpected(double[] factors)
        {
            //act
            Polinome actual = new Polinome(factors);
            //assert
            for (int i = 0; i < factors.Length; i++)
                Assert.LessOrEqual(Math.Abs(factors[i] - actual[i]), epsilon,
                    $"Factor {i} = {actual[i]} differs from expected {factors[i]}");
        }

        [TestCase(new double[] { 5, 0, 0, 12, 15 })]
        [TestCase(new double[] { 1, 2, 3, 4, 5 })]
        [TestCase(new double[] { 0, 0, 0 })]
        [TestCase(new double[] { 0, 0, 2, 3 })]
        [Test]
        public void Polinome_Polinome_InstanceExpected(double[] factors)
        {
            //arrange
            Polinome p = new Polinome(factors);
            //act
            Polinome actual = new Polinome(p);
            //assert
            Assert.AreEqual(p.MaxPower, actual.MaxPower,
                $"{nameof(actual)}'s MaxPower = {actual.MaxPower} " +
                $"differs from expected {p.MaxPower}");
            for (int i = 0; i <= p.MaxPower; i++)
                Assert.LessOrEqual(Math.Abs(p[i] - actual[i]), epsilon,
                    $"Factor {i} = {actual[i]} differs from expected {p[i]}");
        }

        [TestCase(new double[0], typeof(ArgumentException), 
            Description = "Factors array length = 0")]
        [TestCase(null, typeof(ArgumentNullException), 
            Description = "Factors array is null")]
        [Test]
        public void Polinome_Factors_ExceptionExpected
            (double[] factors, Type expectedExceptionType)
        {
            Assert.Throws(expectedExceptionType, () => { new Polinome(factors); });
        }

        [Test]
        public void Polinome_null_ExceptionExpected()
        {
            //arrange
            Polinome p = null;
            //Assert
            Assert.Throws(typeof(ArgumentNullException), () => { new Polinome(p); });
        }

        [TestCase(new double[] { 5, 0, 0, 12, 15 }, 0, 5)]
        [TestCase(new double[] { 5, 0, 0, 12, 15 }, 1, 32)]
        [TestCase(new double[] { 5, 0, 0, 12, 15 }, 2, 341)]
        [TestCase(new double[] { 1, 2, 3, 4, 5 }, 0, 1)]
        [TestCase(new double[] { 1, 2, 3, 4, 5 }, 1, 15)]
        [TestCase(new double[] { 1, 2, 3, 4, 5 }, 1.5, 49.5625)]
        [TestCase(new double[] { 0, 0, 0 }, 3, 0)]
        [TestCase(new double[] { 0, 0, 0 }, 0, 0)]
        [TestCase(new double[] { 0, 0, 0 }, 312321321, 0)]
        [Test]
        public void Count_Polinome_PolinumeValueReturned
            (double[] factors, double x, double expected, double eps = 1e-6)
        {
            //arrange
            Polinome p = new Polinome(factors);
            //act
            double actual = p.Count(x);
            //assert
            Assert.LessOrEqual(Math.Abs(expected - actual), eps,
                $"Counted P(x) = {actual} differ from expected {expected}");
        }

        [TestCase(new double[] { 5, 0, 0, 12, 15 })]
        [TestCase(new double[] { 1, 2, 3, 4, 5 })]
        [TestCase(new double[] { 0, 0, 0 })]
        [TestCase(new double[] { 0, 0, 2, 3 })]
        [Test]
        public void OpMinus_Polinome_minusPolinomReturned(double[] factors)
        {
            //arrange 
            Polinome p = new Polinome(factors);
            //act
            Polinome actual = -p;
            //assert
            for (int i = 0; i < factors.Length; i++)
                Assert.LessOrEqual(Math.Abs(-factors[i] - actual[i]), epsilon);
        }

        [Test]
        public void OpMinus_Polinome_ExceptionExpected()
        {
            //arrange
            Polinome p = null;
            //act
            Assert.Throws(typeof(ArgumentNullException), () => { p = -p; });
        }

        [Test]
        public void OpPlus_Polinome_ExceptionExpected()
        {
            //arrange
            Polinome p = null;
            //act
            Assert.Throws(typeof(ArgumentNullException), () => { p = +p; });
        }
        
        [TestCase(new double[] { 0, 0, 0 }, new double[] { 0, 0, 0 }, new double[] { 0, 0, 0 })]
        [TestCase(new double[] { 0, 0, 0 }, new double[] { 1, 2, 3, 4 }, new double[] { 1, 2, 3, 4 })]
        [TestCase(new double[] { 3, 2, 1 }, new double[] { 1, 2, 3, 4 }, new double[] { 4, 4, 4, 4 })]
        [TestCase(new double[] { -1, -2, -3, 0, 5 }, new double[] { 1, 2, 3, 4 },
            new double[] { 0, 0, 0, 4, 5 })]
        [Test]
        public void OpPlus_Pol1Pol2_SumExpected
            (double[] factors1, double[] factors2, double[] expectedFactors)
        {
            //arrange
            Polinome p1 = new Polinome(factors1);
            Polinome p2 = new Polinome(factors2);
            //act
            Polinome actual = p1 + p2;
            //assert
            for (int i = 0; i < expectedFactors.Length; i++)
                Assert.LessOrEqual(Math.Abs(expectedFactors[i] - actual[i]),
                    epsilon);
        }

        [TestCase(new double[] { 0, 0, 0 }, new double[] { 0, 0, 0 }, new double[] { 0, 0, 0 })]
        [TestCase(new double[] { 0, 0, 0 }, new double[] { 1, 2, 3, 4 }, new double[] { -1, -2, -3, -4 })]
        [TestCase(new double[] { 3, 2, 1 }, new double[] { 1, 2, 3, 4 }, new double[] { 2, 0, -2, -4 })]
        [TestCase(new double[] { -1, -2, -3, 0, 5 }, new double[] { -1, -2, -3, 4 },
            new double[] { 0, 0, 0, -4, 5 })]
        [Test]
        public void OpMinus_Pol1Pol2_SubtractionExpected
            (double[] factors1, double[] factors2, double[] expectedFactors)
        {
            //arrange
            Polinome p1 = new Polinome(factors1);
            Polinome p2 = new Polinome(factors2);
            //act
            Polinome actual = p1 - p2;
            //assert
            for (int i = 0; i < expectedFactors.Length; i++)
                Assert.LessOrEqual(Math.Abs(expectedFactors[i] - actual[i]), epsilon);
        }

        [TestCase(new double[] { 0, 0, 0 }, new double[] { 0, 0, 0 }, new double[] { 0, 0, 0 })]
        [TestCase(new double[] { 0, 0, 0 }, new double[] { 1, 2, 3, 4 }, new double[] { 0, 0, 0, 0 })]
        [TestCase(new double[] { 3, 2, 1 }, new double[] { 1, 2, 4 }, new double[] { 3, 8, 17, 10, 4 })]
        [TestCase(new double[] { -3, -2, -1}, new double[] { -1, -2, -4 },
            new double[] { 3, 8, 17, 10, 4 })]
        [Test]
        public void OpMultiple_Pol1Pol2_CompositionExpected
            (double[] factors1, double[] factors2, double[] expectedFactors)
        {
            //arrange
            Polinome p1 = new Polinome(factors1);
            Polinome p2 = new Polinome(factors2);
            //act
            Polinome actual = p1 * p2;
            //assert
            for (int i = 0; i < expectedFactors.Length; i++)
                Assert.LessOrEqual(Math.Abs(expectedFactors[i] - actual[i]), epsilon);
        }

        [TestCase(new double[] { 0, 1, 2}, "2x^2 + x")]
        [TestCase(new double[] { 0}, "0")]
        [TestCase(new double[] { 2, 3, 4, 5 }, "5x^3 + 4x^2 + 3x + 2")]
        [TestCase(new double[] { -2, 3, -4, 5 }, "5x^3 - 4x^2 + 3x - 2")]
        [TestCase(new double[] { 2, 3, 0, 5 }, "5x^3 + 3x + 2")]
        [TestCase(new double[] { -2, 0, -4, 5 }, "5x^3 - 4x^2 - 2")]
        [Test]
        public void ToString_Polinom_StringRepresentationExpected
            (double[] factors, string expectedString)
        {
            //act
            string actual = new Polinome(factors).ToString();
            //assert
            Assert.IsTrue(string.CompareOrdinal(expectedString, actual) == 0,
                $"Expected \"{expectedString}\" != \"{actual}\"");
        }


    }
}
