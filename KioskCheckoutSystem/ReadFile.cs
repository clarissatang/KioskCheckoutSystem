/*****************************************************************
Filename:       GetOrders.cs
Revised:        Date: 2017/02/23
Revision:       Revision: 1.0.0

Description:    Read in the order info the product database

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


namespace KioskCheckoutSystem.Data
{
    public class ReadFile
    {
        /**********************************
         *  Read in the order file
         *  Category the whole order
         *  e.g. Apple 5, Orange 4, Banana 7
         *  ********************************/
        public Hashtable GetOrders(string orderFile)
        {
            try
            {
                Hashtable orders = new Hashtable();
                string[] allItems = File.ReadAllLines(orderFile);
                foreach (string oneItem in allItems)
                {
                    if (orders.ContainsKey(oneItem) == true) // this item is in the list
                    {
                        int currentNumber = (int)orders[oneItem] + 1;
                        orders[oneItem] = currentNumber;
                    }
                    else // this itme is NOT in the list
                    {
                        orders.Add(oneItem, 1);
                    }
                }

                return orders;
            }
            catch (Exception ex)
            {
                CollectError.CollectErrorToFile(ex, Program.errorFile);
                return null;
            }
        } // END: GetOrders(...)

        /*********************************************************
         * Read in the product database
         * Header is    Product_Name
         *              Regular_Price
         *              is_On_Sale
         *              Sale_Price
         *              is_Additional_Sale
         *              Sale_Rule
         * Save the info in hashtable, Product_Name is the key             
         *******************************************************/
        public Hashtable GetAllProductDatabase(string productDatabasePath)
        {
            try
            {
                Hashtable allProductDatabase = new Hashtable();
                CsvRow row = new CsvRow();
                using (MemoryStream memStream = new MemoryStream())
                using (FileStream fileStream = File.OpenRead(productDatabasePath))
                {
                    memStream.SetLength(fileStream.Length);
                    fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

                    CsvFileReader csvFileReader = new CsvFileReader(memStream);

                    // Read the header, ignore this
                    csvFileReader.ReadRow(row);

                    while (csvFileReader.ReadRow(row))
                    {
                        if (row.Count == 0)
                            break;

                        OneItemData oneItemData = new OneItemData();
                        oneItemData.ItemDataEntry = new string[row.Count];

                        oneItemData.ItemDataEntry = new string[row.Count];
                        for (int a = 0; a < row.Count; a++)
                        {
                            oneItemData.ItemDataEntry[a] = row[a];
                        }

                        allProductDatabase.Add(oneItemData.ItemDataEntry[(int)EnumItemData.ProductName], oneItemData);

                    }
                }

                return allProductDatabase;
            }
            catch (Exception ex)
            {
                CollectError.CollectErrorToFile(ex, Program.errorFile);
                return null;
            }
        }
    }
}
