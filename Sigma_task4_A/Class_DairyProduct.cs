using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sigma_task4_A
{
    public class DairyProduct : Product
    {
        private const double DateCoefficient = 0.01; 

        public DairyProduct(string name, double price, double weight,int expirationDate, string creationDate) 
            : base(name, price, weight, expirationDate,creationDate)
        {
        }
        public DairyProduct(DairyProduct other) : base(other)
        {
        }

        public override double EditPrice(double value)
        {
            double totalValue = value + DateCoefficient * ExpirationDate * Math.Sign(value);
            if (totalValue < -1)
                totalValue = -1;
            Price += Price * totalValue;

            return Price;
        }

        public override string GetProperties()
        {
            //Base contains the same list of properties.
            return base.GetProperties(); 
        }
        public override string ToString()
        {
            return string.Format($"DairyProduct : {Name} {Price} {Weight} {ExpirationDate} {CreationDate}");
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                throw new NullReferenceException("The object has contained null.");

            if (this.GetType() != obj.GetType())
                return false;
            
            var other = obj as DairyProduct;
            return (this.Price == other.Price) && (this.Weight == other.Weight) && (this.Name == other.Name)
                && (this.ExpirationDate == other.ExpirationDate) && (this.CreationDate == other.CreationDate);
        }
        public override object Clone()
        {
            return new DairyProduct(this);
        }
    }
}
