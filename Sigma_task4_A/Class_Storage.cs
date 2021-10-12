using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sigma_task4_A
{
    public class Storage : IEnumerable<Product>
    {
        Product[] ProductList = Array.Empty<Product>();
        public int Count
        {
            get => ProductList.Length;
        }


        public Storage(IEnumerable<Product> products)
        {
            ProductList = new Product[products.Count()];

            for (int i = 0; i < ProductList.Length; i++)
                ProductList[i] = products.ElementAt(i).Clone() as Product;
        }
        public Storage(params Product[] products):this((IEnumerable<Product>)products)
        {

        }
        public Storage()
        {

        }


        public Product this[int index]
        {
            get
            {
                if (index < 0 || index >= ProductList.Length)
                    throw new IndexOutOfRangeException("Index was out of range.");
                return ProductList[index];
            }
            set
            {
                if (index < 0 || index >= ProductList.Length)
                    throw new IndexOutOfRangeException("Index was out of range.");
                ProductList[index] = value;
            }
        }
        public string ContentInfo()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in ProductList)
            {
               stringBuilder.Append(item.GetProperties() + '\n');
            }
            return stringBuilder.ToString();
        }
        public int FillFromConsole(bool append = true)
        {
            var readProducts = ProductManager.GetProductsFromConsole();
            if (append)
                ProductList = Enumerable.Concat(ProductList, readProducts).ToArray();
            else
                ProductList = readProducts;

            return readProducts.Length;
        }
        public void EditPrices(double value)
        {
            foreach (var item in ProductList)
                item.EditPrice(value);
        }
        public void RandomInitialization(int count, int maxPrice = 1000, int maxWeight = 10, int maxExpiration = 1)
        {
            if (count < 0 || maxPrice < 0 || maxWeight < 0 || maxExpiration < 0)
                throw new ArgumentException("Incorrect properties.");

            var randomProductList = new Product[count];
            Random rand = new Random();

            for (int i = 0; i < count; i++)
            {
                switch (rand.Next(3))
                {
                    case 0:
                        randomProductList[i] = new Product($"Product_{i}", rand.NextDouble() * maxPrice,
                            rand.NextDouble() * maxWeight, rand.Next(14), "01.01.2022");
                        break;
                    case 1:
                        randomProductList[i] = new Meat($"Product_{i}", rand.NextDouble() * maxPrice,
                            rand.NextDouble() * maxWeight, rand.Next(14), "01.01.2022", (MeatKind)rand.Next(1, 5),
                            (MeatCategory)rand.Next(1, 4));
                        break;
                    case 2:
                        randomProductList[i] = new DairyProduct($"Product_{i}", rand.NextDouble() * maxPrice,
                            rand.NextDouble() * maxWeight, rand.Next(1, maxExpiration + 1), "01.01.2022");
                        break;
                    default:
                        throw new ApplicationException("Unknown exception.");
                }
            }
        }
        public Product[] GetMeatProducts()
        {
            return ProductList.Where(item => item is Meat).ToArray();
        }
        public Product[] GetDairyProducts()
        {
            return ProductList.Where(item => item is DairyProduct).ToArray();
        }
        public Product[] GetSpecialProducts(Func<Product, bool> predicate)
        {
            return ProductList.Where(predicate).ToArray();
        }
        public int SypplyFromFile(string path)
        {
            IEnumerable<Product> newProducts;
            try
            {
                newProducts = ProductManager.ReadFromFile(path);
                ProductList = Enumerable.Concat(ProductList, newProducts).ToArray();
            }
            catch
            {
                throw;
            }
            return newProducts.Count();
        }
        public int CheckExpiredProducts(string pathToRecords)
        {
            var toBeRemoved = new List<Product>();
            var newList = new List<Product>();
            for (int i = 0; i < ProductList.Length; i++)
            {
                bool isExpired = (ProductList[i].GetType() == typeof(DairyProduct)) 
                    && ProductList[i].ItExpired();
                if (isExpired)
                    toBeRemoved.Add(ProductList[i]);
                else
                    newList.Add(ProductList[i]);
            }
            ProductList = newList.ToArray();

            ProductManager.SaveProductsToFile(toBeRemoved, pathToRecords);
            ProductList = newList.ToArray();

            return toBeRemoved.Count;
        }

        public IEnumerator<Product> GetEnumerator()
        {
            return ((IEnumerable<Product>)ProductList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ProductList.GetEnumerator();
        }
    }
}
