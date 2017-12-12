/*****************************************************************
Filename:       Program.cs
Revised:        Date: 2017/02/23
Revision:       Revision: 1.0.0

Description:    Main function

Revision log:
* 2017-02-21: Created
******************************************************************/

using KioskCheckoutSystem.Data;

namespace KioskCheckoutSystem
{
    public class Program
    {
        public static string ErrorFile;
        private static void Main(string[] args)
        {
            if (args.Length != 4)
                return;
            var orderFilePath = args[0];
            var productDatabasePath = args[1];
            var receiptHeaderFilePath = args[2];
            ErrorFile = args[3];

            var readFile = new ReadFile(orderFilePath, productDatabasePath);

            // Read the order file
            var orders = readFile.GetOrders(); 
            if (orders == null)
                return;

            // Read the product database file
            var allProductData = readFile.GetAllProductData();
            if (allProductData == null)
                return;                   
            
            var checkoutSystem = new CheckoutSystem(allProductData, orders);

            var totalOrderReceipt = checkoutSystem.Checkout();

            totalOrderReceipt.PrintReceipt(receiptHeaderFilePath);
        }
    }
}
