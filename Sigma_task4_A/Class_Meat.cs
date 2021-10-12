using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sigma_task4_A
{
    public class Meat : Product
    {
        private const double HighestGradeCoefficient = 0.15;
        private const double FirstGradeCoefficien = 0.10;
        private const double SecondGradeCoefficient = 0.05;

        public MeatCategory Category { get; set; }
        public MeatKind Kind { get; init; }

        public Meat(string name, double price, double weight, int expirationDate,string creationDate,
            MeatKind kind, MeatCategory category) 
            : base(name,price,weight, expirationDate, creationDate)
        {
            if (!Enum.IsDefined(typeof(MeatKind), kind) || !Enum.IsDefined(typeof(MeatKind), kind))
                throw new ArgumentException("Incorect kind or category value.");
            Category = category;
            Kind = kind;
        }
        public Meat(Meat other) : base(other)
        {
            this.Category = other.Category;
            this.Kind = other.Kind;
        }
        public override double EditPrice(double value)
        {
            if(value < -1)
            {
                Price = 0;
                return Price;
            }
            double totalValue = default;
            switch (Category)       
            {
                case MeatCategory.HighestGrade:
                    totalValue = HighestGradeCoefficient;
                    break;
                case MeatCategory.FirstGrade:
                    totalValue = FirstGradeCoefficien;
                    break;
                case MeatCategory.SecondGrade:
                    totalValue = SecondGradeCoefficient;
                    break;
                default:
                    throw new Exception("Unknown exception.");
            }

            totalValue *= Math.Sign(value);
            totalValue += value;
            Price += Price * totalValue;

            return Price;
        }
        public override string GetProperties()
        {
            return base.GetProperties() + string.Format(" meat kind = {0,-15}, meat category = {1,-15}"); 
        }

        public override string ToString()
        {
            return string.Format($"Meat : {Name} {Price} {Weight} {ExpirationDate} {CreationDate} {Category} {Kind}");
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                throw new NullReferenceException("The object has been null.");

            if (this.GetType() != obj.GetType())
                return false;

            var other = obj as Meat;
            return (this.Price == other.Price) && (this.Weight == other.Weight) && (this.Name == other.Name)
                && (this.ExpirationDate == other.ExpirationDate) && (this.CreationDate == other.CreationDate)
                && (this.Kind == other.Kind) && (this.Category == other.Category);
        }
        public override object Clone()
        {
            return new Meat(this);
        }
    }
}
