using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sigma_task4_A
{
    public class Buy : IEnumerable<Product>
    {
        protected List<Product> ProductList = new List<Product>();
        public int Count 
        {
            get => ProductList.Count; 
        }
        public double TotalPrice { get; private set; } = 0.0;
        public double TotalWeight { get; private set; } = 0.0;
        public Buy(IEnumerable<Product> products)
        {
            ProductList.Capacity = products.Count();
            foreach (var item in products)
            {
                if (item != null)
                {
                    TotalPrice += item.Price;
                    TotalWeight += item.Weight;
                    ProductList.Add(new Product(item));
                }
            }
        }
        public Buy(params Product[] products) : this((IEnumerable<Product>)products)
        {

        }
        public Buy()
        {

        }
        public bool Add(Product newItem)
        {
            if (newItem != null)
            {
                ProductList.Add(newItem);
                TotalPrice += newItem.Price;
                TotalWeight += newItem.Weight;
                return true;
            }
            return false;
        }
        public bool Remove(Product product)
        {
            bool result = ProductList.Remove(product);
            if (result)
            {
                TotalPrice -= product.Price;
                TotalWeight -= product.Weight;
            }
            return result;
        }
        public Product this[int index]
        {
            get
            {
                if(index >= ProductList.Count || index < 0)
                    throw new IndexOutOfRangeException("Index was out of range.");
                return ProductList[index];
            }
            set
            {
                if (index >= ProductList.Count || index < 0)
                    throw new IndexOutOfRangeException("Index was out of range.");
                if (value == null)
                    throw new ArgumentException("Buy must not contain null.");
                ProductList[index] = value;
            }
        }

        IEnumerator<Product> IEnumerable<Product>.GetEnumerator()
        {
            return ProductList.GetEnumerator();
        }
        public IEnumerator GetEnumerator()
        {
            return ProductList.GetEnumerator();
        }
    }
}
