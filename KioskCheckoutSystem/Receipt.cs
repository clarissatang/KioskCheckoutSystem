/*****************************************************************
Filename:       Receipt.cs
Revised:        Date: 2017/02/23
Revision:       1.0.0

Description:    Get the total price and print the detail receipt

Revision log:
* 2017-02-21: Created
******************************************************************/
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

        // Calculate the total price based on the receipt information
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
            try
            {
                string[] receiptHeader = File.ReadAllLines(receiptHeaderFile);
                foreach (string oneLine in receiptHeader)
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
            catch(Exception ex)
            {
                CollectError.CollectErrorToFile(ex, Program.errorFile);
            }
        }
        
    }
}
