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
        public List<Order> GetOrders(string orderFile)
        {
            try
            {
                List<Order> orders = new List<Order>();
                Order oneOrder = new Order();

                string[] all_items = File.ReadAllLines(orderFile);
                foreach (string one_item in all_items)
                {
                    bool isExistInOrders = false;
                    for (int i = 0; i < orders.Count; i++)
                    {
                        if (orders[i].ProductName == one_item)
                        {
                            oneOrder = orders[i];
                            oneOrder.Quantity++;
                            orders[i] = oneOrder;
                            isExistInOrders = true;
                            break;
                        }
                    }
                    if (!isExistInOrders)
                    {
                        oneOrder.ProductName = one_item;
                        oneOrder.Quantity = 1;
                        orders.Add(oneOrder);
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
        public Hashtable GetItemDatabase(string itemDataBaseFile)
        {
            try
            {
                CsvRow row = new CsvRow();
                MemoryStream memStream = new MemoryStream();
                using (FileStream fileStream = File.OpenRead(itemDataBaseFile))
                {
                    memStream.SetLength(fileStream.Length);
                    fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
                }
                CsvFileReader csvFileReader = new CsvFileReader(memStream);

                // Read the header, ignore this
                csvFileReader.ReadRow(row);

                Hashtable itemDatabase = new Hashtable();

                while (csvFileReader.ReadRow(row))
                {
                    if (row.Count == 0)
                        break;

                    IteamDataBase oneItemDatabase = new IteamDataBase();
                    oneItemDatabase.ItemDataEntry = new string[row.Count];

                    oneItemDatabase.ItemDataEntry = new string[row.Count];
                    for (int a = 0; a < row.Count; a++)
                    {
                        oneItemDatabase.ItemDataEntry[a] = row[a];
                    }

                    itemDatabase.Add(oneItemDatabase.ItemDataEntry[(int)EnumItemData.ProductName], oneItemDatabase);

                }
                memStream.Position = 0;
                csvFileReader.DiscardBufferedData();

                return itemDatabase;
            }
            catch (Exception ex)
            {
                CollectError.CollectErrorToFile(ex, Program.errorFile);
                return null;
            }
        }
    }
}
