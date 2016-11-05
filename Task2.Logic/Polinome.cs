using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace Task2.Logic
{
    public sealed class Polinome : IEquatable<Polinome>, ICloneable
    {
        private readonly double[] factors;
        private int capacity;
        private static double epsilon;

        /// <summary>
        /// Real size of <see cref="Polinome"/>
        /// </summary>
        /// <exception cref="ArgumentException">Throws if value is 
        /// less of equal to 0</exception>
        private int Capacity
        {
            get { return capacity; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException
                        ($"{nameof(Capacity)} cannot be less or " +
                         "equal to zero");
                capacity = value;
            }
        }

        public static double Epsilon {
            get { return epsilon; }
            private set
            {
                if (value <= 0 || value >= 1)
                    throw new ArgumentException
                        ($"{nameof(Epsilon)} must be positive " +
                                                "and less than one" );
                epsilon = value;
            } }

        public int MaxPower { get; private set; }

        /// <summary>
        /// Initializes static variables from App.config
        /// </summary>
        static Polinome()
        {
            try
            {
                string epsStr = ConfigurationManager.AppSettings["epsilon"];
                Epsilon = double.Parse(epsStr);
            }
            catch (ConfigurationErrorsException ex)
            {
                throw new ConfigurationErrorsException("Can't get epsilon value", ex);
            }
            catch (FormatException ex)
            {
                throw new ConfigurationErrorsException("epsilon has invalid format", ex);
            }
            catch (Exception ex)
            {
                throw  new ConfigurationErrorsException("epsilon has an invalid value", ex);
            }
        }

        /// <summary>
        /// Creates new <see cref="Polinome"/> according to the array of factors
        /// </summary>
        /// <param name="factors"></param>
        /// <exception cref="ArgumentNullException">Throws if 
        /// <paramref name="factors"/> is null</exception>
        /// <exception cref="ArgumentException">Throws 
        /// if <paramref name="factors"/> length is equal to zero</exception>
        public Polinome(params double[] factors)
        {
            if (factors == null)
                throw new ArgumentNullException($"{nameof(factors)} cannot be null");
            if (factors.Length == 0)
                throw new ArgumentException
                    ($"{nameof(factors.Length)} must be greater than zero");
            Capacity = factors.Length;
            this.factors = new double[Capacity];
            for (int i = 0; i < Capacity; i++)
            {
                this.factors[i] = factors[i];
                if (Math.Abs(factors[i]) > Epsilon)
                    MaxPower = i;
            }
        }

        /// <summary>
        /// Creates new <see cref="Polinome"/> which is the 
        /// same to <paramref name="polinome"/>
        /// </summary>
        /// <param name="polinome"></param>
        /// <exception cref="ArgumentNullException">Throws if
        ///  <paramref name="polinome"/> is null</exception>
        public Polinome(Polinome polinome)
        {
            if (polinome == null)
                throw new ArgumentNullException($"{nameof(polinome)} cannot be null");
            Capacity = polinome.Capacity;
            MaxPower = polinome.MaxPower;
            factors = new double[Capacity];
            for (int i = 0; i <= MaxPower; i++)
                this[i] = polinome[i];
        }

        /// <summary>
        /// Constructs Polinome with setted capacity
        /// </summary>
        /// <param name="capactity"></param>
        private Polinome(int capactity)
        {
            factors = new double[capactity];
            Capacity = capactity;
        }
        /// <summary>
        /// Counts the value of Pn(x)
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double Count(double x)
        {
            double res = 0;
            for (int i = MaxPower; i >= 0; i--)
                if (Math.Abs(this[i]) > Epsilon)
                    res = res + Math.Pow(x, i) * this[i];
            return res;
        }

        /// <exception cref="ArgumentNullException">Throws if 
        /// <paramref name="polinome"/> is null</exception>
        public static Polinome operator +(Polinome polinome)
        {
            if (polinome == null)
                throw new ArgumentNullException
                    ($"{nameof(polinome)} cannot be null");
            return polinome;
        }

        /// <exception cref="ArgumentNullException">Throws if 
        /// <paramref name="polinome"/> is null</exception>
        public static Polinome operator -(Polinome polinome)
        {
            if (polinome == null)
                throw new ArgumentNullException
                    ($"{nameof(polinome)} cannot be null");
            Polinome ret = new Polinome(polinome);
            for (int i = 0; i <= ret.MaxPower; i++)
                ret[i] *= -1;
            return ret;
        }

        /// <exception cref="ArgumentNullException">Throws if 
        /// <paramref name="left"/>  or <paramref name="right"/> is null</exception>
        public static Polinome operator +(Polinome left, Polinome right)
        {
            if (left == null)
                throw new ArgumentNullException($"{nameof(left)} cannot be null");
            if (right == null)
                throw new ArgumentNullException($"{nameof(right)} cannot be null");
            int maxPower = Math.Max(left.MaxPower, right.MaxPower);
            Polinome ret = new Polinome (maxPower + 1);
            ret.MaxPower = maxPower;
            for (int i = 0; i <= maxPower; i++)
                ret[i] = left[i] +  right[i];
            ReduceMaxPower(ret);
            return ret;
        }

        /// <exception cref="ArgumentNullException">Throws if 
        /// <paramref name="left"/>  or <paramref name="right"/> is null</exception>
        public static Polinome operator -(Polinome left, Polinome right)
        {
            return left + (-right);
        }

        /// <exception cref="ArgumentNullException">Throws if 
        /// <paramref name="left"/>  or <paramref name="right"/> is null</exception>
        public static Polinome operator *(Polinome left, Polinome right)
        {
            if (left == null)
                throw new ArgumentNullException($"{nameof(left)} cannot be null");
            if (right == null)
                throw new ArgumentNullException($"{nameof(right)} cannot be null");
            int maxPower = left.MaxPower + right.MaxPower;
            Polinome ret = new Polinome(maxPower + 1);
            ret.MaxPower = maxPower;
            for (int i = 0; i <= left.MaxPower; i++)
                for (int j = 0; j <= right.MaxPower; j++)
                    ret[i + j] = ret[i + j] + left[i] * right[j];
            ReduceMaxPower(ret);
            return ret;
        }

        public static bool operator ==(Polinome left, Polinome right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null))
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(Polinome left, Polinome right)
        {
            return !(left == right);
        }

        /// <exception cref="ArgumentNullException">Throws if 
        /// <paramref name="left"/>  or <paramref name="right"/> is null</exception>
        public static Polinome Add(Polinome left, Polinome right)
        {
            return left + right;
        }

        /// <exception cref="ArgumentNullException">Throws if 
        /// <paramref name="left"/>  or <paramref name="right"/> is null</exception>
        public static Polinome Subtract(Polinome left, Polinome right)
        {
            return left - right;
        }

        /// <exception cref="ArgumentNullException">Throws if 
        /// <paramref name="left"/>  or <paramref name="right"/> is null</exception>
        public static Polinome Multiple(Polinome left, Polinome right)
        {
            return left * right;
        }

        public double this[int power]
        {
            get
            {
                if (power < 0)
                    throw new ArgumentOutOfRangeException
                        ($"{nameof(power)} cannot be less than zero");
                if (power > MaxPower)
                    return 0;
                return factors[power];
            }
            private set
            {
                if (power < 0)
                    throw new ArgumentOutOfRangeException
                        ($"{nameof(power)} cannot be less than zero");
                if (power >= Capacity)
                    throw new ArgumentOutOfRangeException
                        ($"{nameof(power)} cannot be greater than {nameof(Capacity)}");
                
                factors[power] = value;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(MaxPower);
            if (Math.Abs(this[MaxPower]) > Epsilon)
                sb.Append($"{this[MaxPower]:##,###}x^{MaxPower}");
            else
                return "0";
            for (int i = MaxPower - 1; i > 1; i--)
            {
                if (Math.Abs(this[i]) <= Epsilon)
                    continue;
                sb.Append(this[i] < 0 
                    ? $" - {(Math.Abs(this[i] + 1) <= Epsilon ? "" : (-this[i]).ToString("##,###"))}x^{i}" 
                    : $" + {(Math.Abs(this[i] - 1) <= Epsilon ? "" : this[i].ToString("##,###"))}x^{i}");
            }
            if (Math.Abs(this[1]) > Epsilon)
                sb.Append(this[1] < 0
                    ? $" - {(Math.Abs(this[1] + 1) <= Epsilon ? "" : (-this[1]).ToString("##,###"))}x"
                    : $" + {(Math.Abs(this[1] - 1) <= Epsilon ? "" : this[1].ToString("##,###"))}x");
            if (Math.Abs(this[0]) > Epsilon)
                sb.Append(this[0] < 0
                    ? $" - {-this[0]:##,###}"
                    : $" + {this[0]:##,###}");
            return sb.ToString();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 0;
                for (int i = 0; i <= MaxPower; i++)
                    hashCode = CombineHashCodes(hashCode, factors[i].GetHashCode());
                hashCode = (hashCode * 397) ^ MaxPower;
                return hashCode;
            }
        }

        public Polinome Clone()
        {
            return new Polinome(this);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return EqualsHelper(this, (Polinome)obj);
        }

        public bool Equals(Polinome other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (other.GetType() != GetType())
                return false;
            return EqualsHelper(this, other);
        }
        
        private bool EqualsHelper(Polinome p1, Polinome p2)
        {
            if (p1.MaxPower != p2.MaxPower)
                return false;
            for (int i = 0; i < p1.MaxPower; i++)
                if (Math.Abs(p1[i] - p2[i]) > Epsilon)
                    return false;
            return true;
        }

        private static void ReduceMaxPower(Polinome ret)
        {
            while (Math.Abs(ret[ret.MaxPower]) <= Epsilon && ret.MaxPower > 0)
                ret.MaxPower--;
        }

        private static int CombineHashCodes(int h1, int h2)
        {
            return unchecked (((h1 << 5) + h1) ^ h2);
        }
    }
}
