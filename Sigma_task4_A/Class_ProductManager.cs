using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sigma_task4_A
{
    public static class ProductManager
    {
        public static void SaveProductsToFile(IEnumerable<Product> productList, string pathToFile)
        {
            if (productList == null || pathToFile == null)
                throw new ArgumentNullException("Argument cat not be null reference.");
            FileInfo fileInfo = new FileInfo(pathToFile);
            if (fileInfo.Exists)
                fileInfo.Create();

            using (StreamWriter writer = new StreamWriter(pathToFile))
            {
                if (productList.Count() == 0)
                    return;

                foreach (var product in productList)
                    writer.WriteLine(product.ToString());
            }
        }
        public static IEnumerable<Product> ReadFromFile(string pathToFile)
        {
            if (pathToFile == null)
                throw new ArgumentNullException("Argument cat not be null reference.");
            if (!File.Exists(pathToFile))
                throw new FileNotFoundException("File does not exist.");

            List<Product> readProducts = new List<Product>();
            using(StreamReader reader = new StreamReader(pathToFile))
            {
                try
                {
                    while (!reader.EndOfStream)
                        readProducts.Add(Product.Parse(reader.ReadLine()));
                }
                catch
                {
                    throw new FormatException("File contains incorrect input.");
                }
            }
            return readProducts;
        }


        public static Product[] GetProductsFromConsole()
        {
            List<Product> Elements = new List<Product>();
            Console.Clear();

            while (true)
            {
                Console.WriteLine("Will you be going to add new elements? \n Yes - Y | No - N");

                var answer = Console.ReadKey();
                bool userMadeDecision = false;
                while (!userMadeDecision)
                {
                    if (answer.Key == ConsoleKey.N)
                        return Elements.ToArray();
                    else if (answer.Key != ConsoleKey.Y)
                        Console.WriteLine("Invalid input. Try again:");
                    else
                        userMadeDecision = true; 
                }
                Console.Clear();
                Console.WriteLine("Meat - M | DairyProduct - D | Other product - P");
                answer = Console.ReadKey();
                switch (answer.Key)
                {
                    case ConsoleKey.M:
                        Elements.Add(GetMeatFromConsole());
                        break;
                    case ConsoleKey.D:
                        Elements.Add(GetDairyFromConsole());
                        break;
                    case ConsoleKey.P:
                        Elements.Add(GetProductFromConsole());
                        break;
                    default:
                        Console.WriteLine("Incorrect input!");
                        break;
                }
            }
        }
        public static DairyProduct GetDairyFromConsole()
        {
            var basePart = GetProductFromConsole();

            return new DairyProduct(basePart.Name, basePart.Price, basePart.Weight,
                basePart.ExpirationDate, basePart.CreationDate);
        }
        public static Meat GetMeatFromConsole()
        {
            var basePart = GetProductFromConsole();
            MeatCategory category = default;
            MeatKind kind = default;

            bool validInput = false;
            ShowOptions(typeof(MeatCategory));

            while (!validInput)
            {
                Console.Write("Category : ");
                if (Enum.TryParse(Console.ReadLine(), out category) && Enum.IsDefined(typeof(MeatCategory),category))
                    validInput = true;
            }

            validInput = false;
            ShowOptions(typeof(MeatKind));

            while (!validInput)
            {
                Console.Write("Kind : ");
                if (Enum.TryParse(Console.ReadLine(), out kind) && Enum.IsDefined(typeof(MeatKind), kind))
                    validInput = true;
            }

            return new Meat(basePart.Name, basePart.Price, basePart.Weight,
                basePart.ExpirationDate, basePart.CreationDate, kind, category);
        }
        public static Product GetProductFromConsole()
        {
            string name = string.Empty;
            bool validInput = false;
            while(!validInput)
            {
                Console.Write("Name : ");
                name = Console.ReadLine();
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(name))
                    validInput = true;
            }

            double price = GetDoubleValue("Price", x => x >= 0);
            double weight = GetDoubleValue("Weight", x => x > 0);
            int expirationDate = GetIntValue("Expiration date", x => x > 0);

            validInput = false;
            DateTime date = default; 
            while (!validInput)
            {
                Console.Write("Creation date : ");
                if (!DateTime.TryParse(Console.ReadLine(), out  date))
                    validInput = true;
            }

            return new Product(name, price, weight, expirationDate, date.ToString());
        }



        private static void ShowOptions(Type type)
        {
            Array options = default;
            try
            {
                options = Enum.GetValues(type);
            }
            catch
            {
                throw;
            }
            Console.Write("Options : ");
            foreach (var item in options)
                Console.Write($"{item} - {(int)item}  ");
            Console.WriteLine();
        }
        private static double GetDoubleValue(string name, Predicate<double> predicate)
        {
            bool validInput = false;
            double value = default;
            while (!validInput)
            {
                Console.Write($"{name} : ");
                if (double.TryParse(Console.ReadLine(), out value) && predicate(value))
                    validInput = true;
            }
            return value;
        }
        private static int GetIntValue(string name, Predicate<int> predicate)
        {
            bool validInput = false;
            int value = default;
            while (!validInput)
            {
                Console.Write($"{name} : ");
                if (int.TryParse(Console.ReadLine(), out value) && predicate(value))
                    validInput = true;
            }
            return value;
        }
    }
}
