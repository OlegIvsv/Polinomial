using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sigma_task4_A
{
    public sealed class Check
    {
        public static void WriteOnConsole(Product product)
        {
            Console.WriteLine(product?.GetProperties() ?? string.Empty);
        }
        public static void WriteOnConsole(Buy buy, string title = "#-#-# Check #-#-#")
        {
            if(buy == null)
            {
                Console.WriteLine();
                return;
            }

            Console.WriteLine(title);
            for (int i = 0; i < buy.Count; i++)
            {
                Console.WriteLine($"#{i + 1} {buy[i].GetProperties()}");
            }
            Console.WriteLine("COUNT : {0} TOTAL PRICE : {1:F2}$ | TOTAL WEIGHT : {2:F2}", 
                buy.Count, buy.TotalPrice, buy.TotalWeight);
        }
    }
}
