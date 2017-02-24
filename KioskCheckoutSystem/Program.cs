using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KioskCheckoutSystem.Data;

namespace KioskCheckoutSystem
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
                return;
            string orderFilePath = args[0];
            string databaseFilePath = args[1];
            string receiptHeaderFilePath = args[2];

            ReadFile readFile = new ReadFile();
            List<Order> orders = readFile.GetOrders(orderFilePath);
            Hashtable itemDatabase = readFile.GetItemDatabase(databaseFilePath);            

            CheckoutSystem checkoutSystem = new CheckoutSystem(itemDatabase);

            Receipt totalOrderReceipt = checkoutSystem.Checkout(orders);
            
            totalOrderReceipt.PrintReceipt(receiptHeaderFilePath);
            
        }
    }
}
