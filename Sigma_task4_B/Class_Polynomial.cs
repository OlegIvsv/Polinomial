using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sigma_task4_B
{
    public class Polynomial : IEnumerable<Monomial>
    {
        List<Monomial> Components = new List<Monomial>();
        public int Length
        {
            get => Components.Count;
        }

       
        public Polynomial(IEnumerable<Monomial> monomials)
        {
            if (monomials == null)
                throw new ArgumentNullException("Enumerable was null reference.");
            if (monomials.Contains(null))
                throw new ArgumentNullException("Enumerable contsined null reference.");
            foreach (var monomial in monomials)
            {
                int coupleIndex = Components.FindIndex(x => x.M == monomial.M);
                if (coupleIndex >= 0)
                    Components[coupleIndex] = Components[coupleIndex].Plus(monomial);
                else
                    Components.Add(monomial);
            }
            this.RemoveEmptyComponents();
        }
        public Polynomial(Polynomial other)
        {
            if (other == null)
                throw new ArgumentNullException("Argument was null reference.");
            this.Components = other.Components.ToList();
            this.RemoveEmptyComponents();
        }
        public Polynomial()
        {

        }
  
        
        public Polynomial Plus(Polynomial other)
        {
            if (other == null)
                throw new ArgumentNullException("Argument can't be null reference.");

            var jointElements = Enumerable.Concat(this, other);
            return new Polynomial(jointElements);
        }
        public Polynomial Minus(Polynomial other)
        {
            if (other == null)
                throw new ArgumentNullException("Argument can't be null reference.");

            var jointElements = Enumerable.Concat(this, other).ToArray();
            for (int i = 0; i < jointElements.Length; i++)
            {
                jointElements[i] = jointElements[i].Multiply(-1);
            }
            return new Polynomial(jointElements);
        }
        public Polynomial Multiply(Polynomial other)
        {
            if (other == null)
                throw new ArgumentNullException("Argument can't be null reference.");

            Monomial[] components = new Monomial[this.Length * other.Length];
            for (int i = 0; i < other.Length; i++)
            {
                for (int j = 0; j < this.Length; j++)
                {
                    components[i * other.Length + j] = other.Components[i].Multiply(this.Components[j]);
                }
            }
            return new Polynomial(components);
        }
        public static Polynomial Parse(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr) || string.IsNullOrWhiteSpace(inputStr))
                throw new ArgumentException("String doesn't contsin necessary elements or it was null referance.");

            var signs = new List<int>();
            foreach (var symbol in inputStr)
            {
                if (symbol == '+')
                    signs.Add(1);
                if (symbol == '-')
                    signs.Add(0);
            }

            var componentsStr = inputStr.Split(new char[] { '+', '-' }, StringSplitOptions.RemoveEmptyEntries);

            if (componentsStr.Length != signs.Count)
                throw new FormatException("Input string has incorrect format.");

            var components = new Monomial[componentsStr.Length];
            try
            {
                for (int i = 0; i < components.Length; i++)
                {
                    components[i] = Monomial.Parse(componentsStr[i]).Multiply(signs[i] == 1 ? 1 : -1);
                }
                return new Polynomial(components);
            }
            catch
            {
                throw new FormatException("Input string has incorrect format.");
            }
        }
        public double this[int mIndex]
        {
            get
            {
                if (mIndex < 0)
                    throw new IndexOutOfRangeException($"Index was out of range [{0};{Components.Count}) " +
                        $"(index = {mIndex}).");

                int index = Components.FindIndex(x => x.M == mIndex);
                if (index >= 0)
                    return Components[index].C;
                return 0;
            }
            set
            {
                if (mIndex < 0)
                    throw new IndexOutOfRangeException($"Index was out of range [{0};{Components.Count}) " +
                        $"(index = {mIndex}).");
                SetAt(value, mIndex);
            }
        }
        private void SetAt(double newCoefficient, int mIndex)
        {
            if (newCoefficient == 0)
            {
                int index = Components.FindIndex(x => x.M == mIndex);
                if (index >= 0)
                    Components.RemoveAt(index);
            }
            else
            {
                int index = Components.FindIndex(x => x.M == mIndex);
                if (index >= 0)
                    Components[index] = new Monomial(newCoefficient, mIndex);
                else
                    Components.Add(new Monomial(newCoefficient, mIndex));
            }
        }
        public int MaxDegree()
        {
            if (Components.Count == 0)
                return 0;

            int maxM = Components[0].M;
            foreach (var monomial in Components)
                if (maxM < monomial.M)
                    maxM = monomial.M;

            return maxM;
        }
        private void RemoveEmptyComponents() 
            => Components.RemoveAll(m => m.C == 0);


        public static Polynomial operator *(Polynomial first, Polynomial second)
            => first.Multiply(second);
        public static Polynomial operator +(Polynomial first, Polynomial second)
            => first.Multiply(second);
        public static Polynomial operator -(Polynomial first, Polynomial second)
            => first.Minus(second);
 
        public static implicit operator Polynomial(double value)
            => new Polynomial(new Monomial[] { new Monomial(value, 0) });
        

        
        public override string ToString()
        {
            Components.Sort(new MonomialComparerByDegree());
            StringBuilder builder = new StringBuilder();

            foreach (var monomial in Components)
                builder.Append(monomial.ToString());

            return builder.ToString();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                throw new NullReferenceException("Obj must not be null reference.");
            if (obj.GetType() != this.GetType())
                return false;

            var other = obj as Polynomial;
            if (this.Length != other.Length)
                return false;
            for (int i = 0; i < this.Length; i++)
                if (!Components[i].Equals(other.Components[i]))
                    return false;

            return true;
        }
        public IEnumerator<Monomial> GetEnumerator()
        {
            return ((IEnumerable<Monomial>)Components).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Components).GetEnumerator();
        }
    }
}
