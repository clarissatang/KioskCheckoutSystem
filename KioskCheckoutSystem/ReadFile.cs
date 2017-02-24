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
        public List<Order> GetOrders(string orderFile)
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
        } // END: GetOrders(...)

        public Hashtable GetItemDatabase(string itemDataBaseFile)
        {
            CsvRow row = new CsvRow();
            MemoryStream memStream = new MemoryStream();
            using (FileStream fileStream = File.OpenRead(itemDataBaseFile))
            {
                memStream.SetLength(fileStream.Length);
                fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
            }
            CsvFileReader Csv_reader = new CsvFileReader(memStream);

            // Read the header
            Csv_reader.ReadRow(row);

            Hashtable itemDatabase = new Hashtable();
            
            while (Csv_reader.ReadRow(row))
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
            Csv_reader.DiscardBufferedData();
            
            return itemDatabase;
        }
    }
}
