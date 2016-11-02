using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2.Logic
{
    public class Polinome : IEquatable<Polinome>
    {
        private int[] factors;
        private int capacity;

        public int MaxPower { get; private set; }

        public int Capacity
        {
            get { return capacity; }
            private set
            {
                if (value < 0)
                    throw new ArgumentNullException
                        ($"{nameof(Capacity)} cannot be less than zero");
                int[] t = factors;
                factors = new int[value];
                if (t != null)
                    Array.Copy(t, factors, capacity);
                capacity = value;
            }
        }

        public Polinome(int capacity = 10)
        {
            Capacity = capacity;
            MaxPower = 0;
        }

        public Polinome(int[] factors)
        {
            Capacity = factors.Length;
            for (int i = 0; i < factors.Length; i++)
            {
                this.factors[i] = factors[i];
                if (factors[i] != 0)
                    MaxPower = i;
            }
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
                    factors[power] = value;
                Capacity = power*2 + 1;
                factors[power] = value;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(MaxPower);
            sb.Append($"{this[MaxPower]}x^{MaxPower}");
            for (int i = MaxPower - 1; i > 0; i--)
            {
                if (this[i] == 0)
                    continue;
                sb.Append(this[i] < 0 
                    ? $" - {-this[i]}x^{i}" 
                    : $" + {this[i]}x^{i}");
            }
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

        private static int CombineHashCodes(int h1, int h2)
        {
            return unchecked (((h1 << 5) + h1) ^ h2);
        }
    }
}
