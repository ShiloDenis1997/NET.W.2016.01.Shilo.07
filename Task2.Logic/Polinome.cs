using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Task2.Logic
{
    public class Polinome : IEquatable<Polinome>
    {
        private int[] factors;
        private int capacity;

        public int MaxPower { get; private set; }

        /// <summary>
        /// Real size of <see cref="Polinome"/>
        /// </summary>
        /// <exception cref="ArgumentException">Throws if value is 
        /// less of equal to 0</exception>
        public int Capacity
        {
            get { return capacity; }
            private set
            {
                if (value <= 0)
                    throw new ArgumentException
                        ($"{nameof(Capacity)} cannot be less or equal to zero");
                int[] t = factors;
                factors = new int[value];
                if (t != null)
                    Array.Copy(t, factors, capacity);
                capacity = value;
            }
        }

        /// <summary>
        /// Creates new <see cref="Polinome"/> with 
        /// <paramref name="capacity"/> size
        /// </summary>
        /// <param name="capacity"></param>
        /// <exception cref="ArgumentException">Throws if 
        /// <paramref name="capacity"/> is less or equal to zero</exception>
        public Polinome(int capacity = 10)
        {
            if (capacity <= 0)
                throw new ArgumentException
                    ($"{nameof(capacity)} must be greater than zero");
            Capacity = capacity;
            MaxPower = 0;
        }

        /// <summary>
        /// Creates new <see cref="Polinome"/> according to the array of factors
        /// </summary>
        /// <param name="factors"></param>
        /// <exception cref="ArgumentNullException">Throws if 
        /// <paramref name="factors"/> is null</exception>
        /// <exception cref="ArgumentException">Throws 
        /// if <paramref name="factors"/> length is equal to zero</exception>
        public Polinome(int[] factors)
        {
            if (factors == null)
                throw new ArgumentNullException($"{nameof(factors)} cannot be null");
            if (factors.Length == 0)
                throw new ArgumentException
                    ($"{nameof(factors.Length)} must be greater than zero");
            Capacity = factors.Length;
            for (int i = 0; i < factors.Length; i++)
            {
                this.factors[i] = factors[i];
                if (factors[i] != 0)
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
            for (int i = 0; i <= MaxPower; i++)
                this[i] = polinome[i];
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
                res = res + Math.Pow(x, i)*this[i];
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
            for (int i = 0; i <= maxPower; i++)
                ret[i] = checked (left[i] + right[i]);
            ReduceMaxPower(ret);
            return ret;
        }

        /// <exception cref="ArgumentNullException">Throws if 
        /// <paramref name="left"/>  or <paramref name="right"/> is null</exception>
        public static Polinome operator -(Polinome left, Polinome right)
        {
            if (left == null)
                throw new ArgumentNullException($"{nameof(left)} cannot be null");
            if (right == null)
                throw new ArgumentNullException($"{nameof(right)} cannot be null");
            int maxPower = Math.Max(left.MaxPower, right.MaxPower);
            Polinome ret = new Polinome(maxPower + 1);
            for (int i = 0; i <= maxPower; i++)
                ret[i] = checked(left[i] - right[i]);
            ReduceMaxPower(ret);
            return ret;
        }

        /// <exception cref="ArgumentNullException">Throws if 
        /// <paramref name="left"/>  or <paramref name="right"/> is null</exception>
        public static Polinome operator *(Polinome left, Polinome right)
        {
            if (left == null)
                throw new ArgumentNullException($"{nameof(left)} cannot be null");
            if (right == null)
                throw new ArgumentNullException($"{nameof(right)} cannot be null");
            int maxPower = left.MaxPower * right.MaxPower;
            Polinome ret = new Polinome(maxPower + 1);
            for (int i = 0; i <= left.MaxPower; i++)
                for (int j = 0; j <= right.MaxPower; j++)
                    ret[i + j] = checked(ret[i + j] + left[i] * right[j]);
            ReduceMaxPower(ret);
            return ret;
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

        public int this[int power]
        {
            get
            {
                if (power < 0)
                    throw new IndexOutOfRangeException
                        ($"{nameof(power)} cannot be less than zero");
                if (power > MaxPower)
                    return 0;
                return factors[power];
            }
            set
            {
                if (power < 0)
                    throw new IndexOutOfRangeException
                        ($"{nameof(power)} cannot be less than zero");
                if (power < capacity)
                {
                    factors[power] = value;
                }
                else
                {
                    Capacity = power*2 + 1;
                    factors[power] = value;
                }
                if (power > MaxPower && factors[power] != 0)
                    MaxPower = power;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(MaxPower);
            if (this[MaxPower] != 0)
                sb.Append($"{this[MaxPower]}x^{MaxPower}");
            else
                return "0";
            for (int i = MaxPower - 1; i > 1; i--)
            {
                if (this[i] == 0)
                    continue;
                sb.Append(this[i] < 0 
                    ? $" - {(-this[i] == 1 ? "" : (-this[i]).ToString())}x^{i}" 
                    : $" + {(this[i] == 1 ? "" : this[i].ToString())}x^{i}");
            }
            if (this[1] != 0)
                sb.Append(this[1] < 0
                    ? $" - {(-this[1] == 1 ? "" : (-this[1]).ToString())}x"
                    : $" + {(this[1] == 1 ? "" : this[1].ToString())}x");
            if (this[0] != 0)
                sb.Append(this[0] < 0
                    ? $" - {-this[0]}"
                    : $" + {this[0]}");
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

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return EqualsHelper(this, (Polinome)obj);
        }

        public bool Equals(Polinome other)
        {
            if (other == null)
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
                if (p1[i] != p2[i])
                    return false;
            return true;
        }

        private static void ReduceMaxPower(Polinome ret)
        {
            while (ret[ret.MaxPower] == 0 && ret.MaxPower > 0)
                ret.MaxPower--;
        }

        private static int CombineHashCodes(int h1, int h2)
        {
            return unchecked (((h1 << 5) + h1) ^ h2);
        }
    }
}
