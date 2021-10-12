using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sigma_task4_B
{
    public class Monomial
    {
        public readonly double C;
        public readonly int M;


        public Monomial(double c, int m)
        {
            if (m < 0)
                throw new ArgumentException("Incorrect degree.");
            C = c;
            M = m;
        }
        public Monomial(Monomial other)
        {
            if (other == null)
                throw new ArgumentNullException("Argument must not be null reference.");

            this.M = other.M;
            this.C = other.C;
        }


        public Monomial Plus(Monomial other)
        {
            if (other == null)
                throw new NullReferenceException("Argument must not be null reference.");
            if (this.M != other.M)
                return null;
            return new Monomial(this.C + other.C, this.M);
        }
        public Monomial Minus(Monomial other)
        {
            if (other == null)
                throw new ArgumentNullException("Argument must not be null reference.");
            if (this.M != other.M)
                return null;
            return new Monomial(this.C - other.C, this.M);
        }
        public Monomial Multiply(Monomial other)
        {
            if (other == null)
                throw new ArgumentNullException("Argument must not be null reference.");

            return new Monomial(this.C * other.C, this.M + other.M);
        }
        public static Monomial Parse(string inputStr)
        {
            if (inputStr == null)
                throw new ArgumentNullException("Argument must not be null reference.");

            var parts = inputStr.Split('x', StringSplitOptions.TrimEntries);
            if (parts.Length != 2)
                throw new FormatException("Incorrect input string.");

            if (!double.TryParse(parts[0], out double c))
                throw new FormatException("Incorrect input string.");
            if (parts[1][0] != '^' || !uint.TryParse(parts[1].Remove(0, 1), out uint m))
                throw new FormatException("Incorrect input string.");

            return new Monomial(c, Convert.ToInt32(m));
        }
        public Monomial Multiply(int n)
        {
            return new Monomial(this.C * n, M);
        }


        public override string ToString()
        {
            return string.Format("{0:+#;-#;+0}x^{1}", C, M);
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                throw new NullReferenceException("Obj must not be null reference.");
            if (obj.GetType() != this.GetType())
                return false;

            var other = obj as Monomial;
            return this.C == other.C && this.M == other.M;
        }
    }
}
