/*****************************************************************
Filename:       Program.cs
Revised:        Date: 2017/02/23
Revision:       Revision: 1.0.0

Description:    Main function

Revision log:
* 2017-02-21: Created
******************************************************************/

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
        public static string errorFile;
        static void Main(string[] args)
        {
            if (args.Length != 4)
                return;
            string orderFilePath = args[0];
            string databaseFilePath = args[1];
            string receiptHeaderFilePath = args[2];
            errorFile = args[3];

            ReadFile readFile = new ReadFile();

            // Read the order file
            List<Order> orders = readFile.GetOrders(orderFilePath);
            if (orders == null)
                return;

            // Read the product database file
            Hashtable itemDatabase = readFile.GetItemDatabase(databaseFilePath);
            if (itemDatabase == null)
                return;                   
            
            CheckoutSystem checkoutSystem = new CheckoutSystem(itemDatabase);
            if (checkoutSystem == null)
                return;

            Receipt totalOrderReceipt = checkoutSystem.Checkout(orders);
            if (totalOrderReceipt == null)
                return;

            totalOrderReceipt.PrintReceipt(receiptHeaderFilePath);
            
        }
    }
}
