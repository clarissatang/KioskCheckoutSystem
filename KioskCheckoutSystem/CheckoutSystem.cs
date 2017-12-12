/*****************************************************************
Filename:       CheckoutSystem.cs
Revised:        Date: 2017/02/23
Revision:       Revision: 1.0.0

Description:    Calculate

Revision log:
* 2017-02-21: Created
******************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KioskCheckoutSystem
{
    public class CheckoutSystem
    {
        private readonly Hashtable _allProductData;
        private readonly Hashtable _orders;
        public  CheckoutSystem(Hashtable allProductData, Hashtable orders)
        {
            _allProductData = allProductData;
            _orders = orders;
        }
        
        public Receipt Checkout()
        {
            try
            {
                var receipt = new Receipt
                {
                    SingleItemReceiptList = new List<SingleItemReceipt>()
                };

                foreach (string productName in _orders.Keys)
                {
                    var oneOrderReceipt = CalculateOneItemPrice(productName, (int)_orders[productName]);
                    receipt.SingleItemReceiptList = receipt.SingleItemReceiptList.Concat(oneOrderReceipt).ToList();
                }
                
                return receipt;
            }
            catch (Exception ex)
            {                
                CollectError.CollectErrorToFile(ex, Program.ErrorFile);
                return null;
            }

        }
        
        private IEnumerable<SingleItemReceipt> CalculateOneItemPrice(string productName, int quantity)
        {
            try
            {                
                var thisProductData = (ProductModel)_allProductData[productName];

                var groupOneItemReceipt = new List<SingleItemReceipt>();

                if (thisProductData.IsOnSale)
                {
                    var promotion = new Promotion(thisProductData);
                    groupOneItemReceipt = promotion.OnSaleItemReceipt(quantity);
                }
                else
                {
                    for (var i = 0; i < quantity; i++)
                    {
                        var oneItemReceipt = new SingleItemReceipt
                        {
                            ProductName = thisProductData.Name,
                            RegularPrice = thisProductData.RegularPrice,
                            Saving = 0
                        };
                        groupOneItemReceipt.Add(oneItemReceipt);
                    }
                }
                return groupOneItemReceipt;
                
            }
            catch (Exception ex)
            {
                CollectError.CollectErrorToFile(ex, Program.ErrorFile);
                return null;
            }

        }
        
    }
}
