using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sigma_task4_A
{
    public class Product : ICloneable
    {
        private const string CreationDateFormat = "d";
        public string Name { get; private set; }
        public double Price { get; protected set; }
        public double Weight { get; init; }
        public int ExpirationDate { get; private set; }
        public readonly string CreationDate;

        public Product(string name, double price, double weight, int expirationDate, string creationDate)
        {
            if (price < 0 || weight <= 0 || expirationDate <= 0)
                throw new ArgumentException("Invalid properties.");
            Price = price;
            Weight = weight;
            ExpirationDate = expirationDate;
            Name = string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name) ? "Unknown product" : name;

            if (!DateTime.TryParse(creationDate, out DateTime date))
                throw new ArgumentException("Incorrect creation date string.");
            CreationDate = date.ToString(CreationDateFormat);
        }
        public Product(in Product other)
            : this(other.Name, other.Price, other.Weight, other.ExpirationDate, other.CreationDate)
        {
        }

        public virtual double EditPrice(double value)
        {
            if (value < -1)
                value = -1;
            Price += Price * value;

            return Price;
        }
        public bool ItExpired()
        {
            var lifeTime = DateTime.Now - DateTime.Parse(CreationDate);
            if (lifeTime.Days >= ExpirationDate)
                return true;
            return false;
        }
        public static Product Parse(string inputStr)
        {
            // Format must be like: "namePrice weight expirationDate creationDate;".
            // If string describes Meat object, it must also contain "... meatKind, meatCategory". 

            var parts = inputStr.Split(':', StringSplitOptions.TrimEntries);
            if (parts.Length != 2)
                throw new FormatException("Input string has incorrect format.");

            var propertiesParts = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (propertiesParts.Length != (parts[0] == "Meat" ? 7 : 5))
                throw new FormatException("Input string has incorrect format.");

            if (!double.TryParse(propertiesParts[1], out double price)
                || !double.TryParse(propertiesParts[2], out double weight)
                || !int.TryParse(propertiesParts[3], out int expirationDate))
                throw new FormatException("Input string has incorrect format.");

            MeatKind meatKind = default;
            MeatCategory meatCategory = default;
            if (parts[0] == "Meat" && (!Enum.TryParse(parts[5], out meatKind)
                || !Enum.TryParse(parts[6], out meatCategory)))
                throw new FormatException("Input string has incorrect format.");

            try
            {
                switch (parts[0])
                {
                    case "Meat":
                        return new Meat(propertiesParts[0], price, weight, expirationDate,
                            propertiesParts[4], meatKind, meatCategory);
                    case "DairyProduct":
                        return new DairyProduct(propertiesParts[0], price, weight, expirationDate,
                            propertiesParts[4]);
                    default:
                        return new Product(propertiesParts[0], price, weight, expirationDate,
                            propertiesParts[4]);

                }
            }
            catch
            {
                throw new FormatException("Input string has incorrect format.");
            }
        }
        public virtual string GetProperties()
        {
            return string.Format("{0,-20}: price = {1,-10:F2}, weight = {2,-10:F2}," +
                "expiration date = {3,-5:D}, creation date = {4,-10}.",
                Name, Price, Weight, ExpirationDate, CreationDate);
        }


        public override string ToString()
        {
            return string.Format($"Product : {Name} {Price} {Weight} {ExpirationDate} {CreationDate}");
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                throw new NullReferenceException("The object has been null.");

            if (this.GetType() != obj.GetType())
                return false;

            var other = obj as Product;
            return this.Name == other.Name;
        }
        public virtual object Clone()
        {
            return new Product(this);
        }


        
    }
}
