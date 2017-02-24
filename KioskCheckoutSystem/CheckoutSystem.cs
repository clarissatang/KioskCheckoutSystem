/*****************************************************************
Filename:       CheckoutSystem.cs
Revised:        Date: 2017/02/23
Revision:       Revision: 1.0.0

Description:    Calculate

Revision log:
* 2017-02-21: Created
******************************************************************/
using KioskCheckoutSystem.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KioskCheckoutSystem
{
    public class CheckoutSystem
    {
        private Hashtable mItemDatabase;
        public  CheckoutSystem(Hashtable itemDatabase)
        {
            mItemDatabase = itemDatabase;
        }
        
        public Receipt Checkout(List<Order> orders)
        {
            try
            {
                Receipt receipt = new Receipt();
                receipt.singleItemReceiptList = new List<SingleItemReceipt>();

                foreach (Order oneOrder in orders)
                {
                    List<SingleItemReceipt> oneOrderReceipt = CalculateOneItemPrice(oneOrder);
                    receipt.singleItemReceiptList = receipt.singleItemReceiptList.Concat(oneOrderReceipt).ToList();
                }

                return receipt;
            }
            catch (Exception ex)
            {                
                CollectError.CollectErrorToFile(ex, Program.errorFile);
                return null;
            }

        } // END: public decimal calculate_total_price(...)
        
        //Get the receipt for one item
        private List<SingleItemReceipt> CalculateOneItemPrice(Order oneOrder)
        {
            try
            {
                int quantity = oneOrder.Quantity;
                IteamDataBase oneItemDataBase = (IteamDataBase)mItemDatabase[oneOrder.ProductName];
                bool isOnSale = oneItemDataBase.ItemDataEntry[(int)EnumItemData.isOnSale].Equals("Yes", StringComparison.CurrentCultureIgnoreCase);
                bool isOnAdditionalSale = oneItemDataBase.ItemDataEntry[(int)EnumItemData.isAdditionalSale].Equals("Yes", StringComparison.CurrentCultureIgnoreCase);

                List<SingleItemReceipt> groupOneItemReceipt = new List<SingleItemReceipt>();

                if (isOnSale == true)
                {
                    Promotion promotion = new Promotion();
                    groupOneItemReceipt = promotion.OnSaleItem(quantity, oneItemDataBase);
                }
                else // not on sale, use regular price
                {
                    for (int i = 0; i < quantity; i++)
                    {
                        SingleItemReceipt oneItemReceipt = new SingleItemReceipt();
                        oneItemReceipt.ProductName = oneItemDataBase.ItemDataEntry[(int)EnumItemData.ProductName];
                        oneItemReceipt.RegularPrice = Convert.ToDecimal(oneItemDataBase.ItemDataEntry[(int)EnumItemData.RegularPrice]);
                        oneItemReceipt.Saving = 0;
                        groupOneItemReceipt.Add(oneItemReceipt);
                    }
                }
                return groupOneItemReceipt;
            }
            catch (Exception ex)
            {
                CollectError.CollectErrorToFile(ex, Program.errorFile);
                return null;
            }

        } // END: CalculateOneItemPrice(...)
        
    }
}
