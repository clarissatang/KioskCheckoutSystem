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
using System.IO;

namespace KioskCheckoutSystem.Data
{
    public class ReadFile
    {
        /**********************************
         *  Read in the order file
         *  Category the whole order
         *  e.g. Apple 5, Orange 4, Banana 7
         *  ********************************/
        private readonly string _orderFile;
        private readonly string _allProductDataFile;

        public ReadFile(string orderFilePath, string productDatabasePath)
        {
            _orderFile = orderFilePath;
            _allProductDataFile = productDatabasePath;
        }

        public Hashtable GetOrders()
        {
            try
            {
                var orders = new Hashtable();
                var allItems = File.ReadAllLines(_orderFile);
                foreach (var oneItem in allItems)
                {
                    if (orders.ContainsKey(oneItem))
                    {
                        orders[oneItem] = (int)orders[oneItem] + 1;
                    }
                    else
                    {
                        orders.Add(oneItem, 1);
                    }
                }
                return orders;
            }
            catch (Exception ex)
            {
                CollectError.CollectErrorToFile(ex, Program.ErrorFile);
                return null;
            }
        }

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
        public Hashtable GetAllProductData()
        {
            try
            {
                var allProductData = new Hashtable();
                var row = new CsvRow();
                using (var memStream = new MemoryStream())
                using (var fileStream = File.OpenRead(_allProductDataFile))
                {
                    memStream.SetLength(fileStream.Length);
                    fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);

                    var csvFileReader = new CsvFileReader(memStream);

                    // Read the header, ignore this
                    csvFileReader.ReadRow(row);

                    while (csvFileReader.ReadRow(row))
                    {
                        if (row.Count == 0)
                            break;
                        var oneProductData = new ProductModel()
                        {
                            Name = row[0],
                            RegularPrice = Convert.ToDecimal(row[1]),
                            IsOnSale = row[2].Equals("Yes",StringComparison.CurrentCultureIgnoreCase),
                            SalePrice = Convert.ToDecimal(row[3]),
                            IsAdditionalSale = row[4].Equals("Yes", StringComparison.CurrentCultureIgnoreCase),
                            SaleRule = row[5]
                        };

                        allProductData.Add(oneProductData.Name, oneProductData);
                    }
                }
                return allProductData;
            }
            catch (Exception ex)
            {
                CollectError.CollectErrorToFile(ex, Program.ErrorFile);
                return null;
            }
        }
    }
}
