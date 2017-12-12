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

namespace KioskCheckoutSystem
{
    public class Receipt
    {
        public List<SingleItemReceipt> SingleItemReceiptList { get; set; }

        // Calculate the total price based on the receipt information
        public decimal TotalPrice
        {
            get
            {
                return SingleItemReceiptList.Sum(s => s.RegularPrice - s.Saving);
            }
        }

        public void PrintReceipt(string receiptHeaderFile)
        {
            try
            {
                var receiptHeader = File.ReadAllLines(receiptHeaderFile);
                foreach (var oneLine in receiptHeader)
                {
                    Console.WriteLine(oneLine);
                }

                foreach (var singleItemReceipt in SingleItemReceiptList)
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
                CollectError.CollectErrorToFile(ex, Program.ErrorFile);
            }
        }
        
    }
}
