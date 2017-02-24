using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskCheckoutSystem
{
    public class Receipt
    {
        public List<SingleItemReceipt> singleItemReceiptList { get; set; }
        public decimal TotalPrice
        {
            get
            {
                decimal totalPrice = 0.0m;
                foreach (SingleItemReceipt singleItemReceipt in singleItemReceiptList)
                {
                    totalPrice += singleItemReceipt.RegularPrice - singleItemReceipt.Saving;
                }
                return totalPrice;
            }
        }

        public void PrintReceipt(string receiptHeaderFile)
        {
            string[] receiptHeader = File.ReadAllLines(receiptHeaderFile);
            foreach(string oneLine in receiptHeader)
            {
                Console.WriteLine(oneLine);
            }

            foreach (SingleItemReceipt singleItemReceipt in singleItemReceiptList)
            {
                if (singleItemReceipt.ProductName.Length > 7)
                    Console.WriteLine("{0}\t{1}\t\t{2}", singleItemReceipt.ProductName,
                        singleItemReceipt.RegularPrice,
                        singleItemReceipt.Saving);
                else
                    Console.WriteLine("{0}\t\t{1}\t\t{2}", singleItemReceipt.ProductName,
                    singleItemReceipt.RegularPrice,
                    singleItemReceipt.Saving);
            }
            Console.WriteLine("Total Price is ${0}", TotalPrice);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        
    }
}
