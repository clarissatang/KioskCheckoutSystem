﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KioskCheckoutSystem.Data;

namespace KioskCheckoutSystem
{
    public class Promotion
    {
        const string GROUPSALE = "Group";
        const string BUYMORESALE = "Buy More";

        private struct GroupSaleInfo
        {
            public int groupQuantity;
            public decimal groupPrice;
        };

        private struct BuyMoreSaleInfo
        {
            public int regularPriceQuantity;
            public int salePriceQuantity;
            public decimal discountPercent;
        };
        
        private List<SingleItemReceipt> GetSaleRuleReceipt(int quantity, IteamDataBase oneItemDataBase)
        {
            List<SingleItemReceipt> groupOneItemReceipt = new List<SingleItemReceipt>();
            for (int i = 0; i < quantity; i++)
            {
                SingleItemReceipt oneItemReceipt = new SingleItemReceipt();
                oneItemReceipt.ProductName = oneItemDataBase.ItemDataEntry[(int)EnumItemData.ProductName];
                oneItemReceipt.RegularPrice = Convert.ToDecimal(oneItemDataBase.ItemDataEntry[(int)EnumItemData.RegularPrice]);
                oneItemReceipt.Saving = oneItemReceipt.RegularPrice - Convert.ToDecimal(oneItemDataBase.ItemDataEntry[(int)EnumItemData.SalePrice]);
                groupOneItemReceipt.Add(oneItemReceipt);
            }
            return groupOneItemReceipt;
        }
        private List<SingleItemReceipt> GetSaleRuleReceipt(int quantity, IteamDataBase oneItemDataBase, BuyMoreSaleInfo buyMoreSaleInfo)
        {
            string productName = oneItemDataBase.ItemDataEntry[(int)EnumItemData.ProductName];
            decimal regularPrice = Convert.ToDecimal(oneItemDataBase.ItemDataEntry[(int)EnumItemData.RegularPrice]);
            List<SingleItemReceipt> groupOneItemReceipt = new List<SingleItemReceipt>();
            int groupNumber = quantity / (buyMoreSaleInfo.regularPriceQuantity + buyMoreSaleInfo.salePriceQuantity);
            int outofGroupQuantity = quantity % (buyMoreSaleInfo.regularPriceQuantity + buyMoreSaleInfo.salePriceQuantity);

            int quo = outofGroupQuantity / buyMoreSaleInfo.regularPriceQuantity;
            int rem = outofGroupQuantity % buyMoreSaleInfo.regularPriceQuantity;

            int regularPriceNumber = 0;
            
            if (quo == 0)
            {
                regularPriceNumber = groupNumber * buyMoreSaleInfo.regularPriceQuantity + rem;
            }
            else
            {
                regularPriceNumber = (groupNumber + quo) * buyMoreSaleInfo.regularPriceQuantity;
            }
            int salePriceNumber = quantity - regularPriceNumber;

            for (int i = 0; i < regularPriceNumber; i++)
            {
                SingleItemReceipt oneItemReceipt = new SingleItemReceipt();
                oneItemReceipt.ProductName = productName;
                oneItemReceipt.RegularPrice = regularPrice;
                oneItemReceipt.Saving = 0;
                groupOneItemReceipt.Add(oneItemReceipt);
            }

            for (int i = 0; i < salePriceNumber; i++)
            {
                SingleItemReceipt oneItemReceipt = new SingleItemReceipt();
                oneItemReceipt.ProductName = productName;
                oneItemReceipt.RegularPrice = regularPrice;
                if (buyMoreSaleInfo.discountPercent == 1) // it is free
                {
                    oneItemReceipt.Saving = regularPrice;
                }
                else
                {
                    oneItemReceipt.Saving = Math.Round(regularPrice * buyMoreSaleInfo.discountPercent,2);
                }
                groupOneItemReceipt.Add(oneItemReceipt);
            }
            
            return groupOneItemReceipt;
        }

        private List<SingleItemReceipt> GetSaleRuleReceipt(int quantity, IteamDataBase oneItemDataBase, GroupSaleInfo groupSaleInfo)
        {
            string productName = oneItemDataBase.ItemDataEntry[(int)EnumItemData.ProductName];
            decimal regularPrice = Convert.ToDecimal(oneItemDataBase.ItemDataEntry[(int)EnumItemData.RegularPrice]);
            List<SingleItemReceipt> groupOneItemReceipt = new List<SingleItemReceipt>();
            int groupNumber = quantity / groupSaleInfo.groupQuantity;
            int outOfGroupQuantity = quantity % groupSaleInfo.groupQuantity;
            for (int i = 0; i < groupNumber; i++) // use group price
            {
                decimal priceTotalTmp = 0.0m;
                for (int j = 0; j < groupSaleInfo.groupQuantity; j++)
                {
                    SingleItemReceipt oneItemReceipt = new SingleItemReceipt();
                    oneItemReceipt.ProductName = productName;
                    oneItemReceipt.RegularPrice = regularPrice;
                    decimal salePrice;

                    if (j == groupSaleInfo.groupQuantity - 1) // this is the last one, price might be different, e.g., buy 3 for $2
                    {
                        salePrice = groupSaleInfo.groupPrice - priceTotalTmp;
                    }
                    else
                    {
                        salePrice = Math.Round(groupSaleInfo.groupPrice / groupSaleInfo.groupQuantity, 2);
                        priceTotalTmp += salePrice;
                    }
                    oneItemReceipt.Saving = regularPrice - salePrice;
                    groupOneItemReceipt.Add(oneItemReceipt);
                }
            }

            for (int i = 0; i < outOfGroupQuantity; i++) // use regular price
            {
                SingleItemReceipt oneItemReceipt = new SingleItemReceipt();
                oneItemReceipt.ProductName = productName;
                oneItemReceipt.RegularPrice = regularPrice;
                oneItemReceipt.Saving = 0;
                groupOneItemReceipt.Add(oneItemReceipt);
            }
            return groupOneItemReceipt;
        }
        public List<SingleItemReceipt> OnSaleItem(int quantity, IteamDataBase oneItemDataBase)
        {
            bool isOnAdditionalSale = oneItemDataBase.ItemDataEntry[(int)EnumItemData.isAdditionalSale].Equals("Yes", StringComparison.CurrentCultureIgnoreCase);
            List<SingleItemReceipt> groupOneItemReceipt = new List<SingleItemReceipt>();

            if (isOnAdditionalSale == true)
            {
                string saleRule = oneItemDataBase.ItemDataEntry[(int)EnumItemData.SaleRule];

                if (saleRule.StartsWith(BUYMORESALE))
                {
                    BuyMoreSaleInfo buyMoreSaleInfo = GetBuyMoreSaleDetail(saleRule);
                    groupOneItemReceipt = GetSaleRuleReceipt(quantity, oneItemDataBase, buyMoreSaleInfo);
                }

                if (saleRule.StartsWith(GROUPSALE))
                {
                    GroupSaleInfo groupSaleInfo = GetGroupSaleDetail(saleRule);
                    groupOneItemReceipt = GetSaleRuleReceipt(quantity, oneItemDataBase, groupSaleInfo);

                }
            }
            else // regular sale
            {
                groupOneItemReceipt = GetSaleRuleReceipt(quantity, oneItemDataBase);
            }
            
            return groupOneItemReceipt;
        } //END: OnSaleAdditional()

        private GroupSaleInfo GetGroupSaleDetail(string saleRule)
        {
            GroupSaleInfo groupSaleInfo = new GroupSaleInfo();
            // format in database: Group: buy [3] for $[2]
            int index_start = saleRule.IndexOf("[");
            int index_end = saleRule.IndexOf("]");
            string groupQuantityString = "";
            for(int i = index_start + 1; i < index_end; i++)
            {
                groupQuantityString += saleRule[i];
            }
            groupSaleInfo.groupQuantity = Convert.ToInt32(groupQuantityString);

            saleRule = saleRule.Substring(index_end + 1);
            index_start = saleRule.IndexOf("[");
            index_end = saleRule.IndexOf("]");
            string groupPriceString = "";
            for (int i = index_start + 1; i < index_end; i++)
            {
                groupPriceString += saleRule[i];
            }
            groupSaleInfo.groupPrice = Convert.ToDecimal(groupPriceString);
            return groupSaleInfo;
        }

        private BuyMoreSaleInfo GetBuyMoreSaleDetail(string saleRule)
        {
            BuyMoreSaleInfo buyMoreSaleInfo = new BuyMoreSaleInfo();
            // format in database: Additional: buy [1] get [1] [50%] off
            int index_start = saleRule.IndexOf("[");
            int index_end = saleRule.IndexOf("]");
            string regularPriceQuantityString = "";
            for (int i = index_start + 1; i < index_end; i++)
            {
                regularPriceQuantityString += saleRule[i];
            }
            buyMoreSaleInfo.regularPriceQuantity = Convert.ToInt32(regularPriceQuantityString);

            saleRule = saleRule.Substring(index_end + 1);
            index_start = saleRule.IndexOf("[");
            index_end = saleRule.IndexOf("]");
            string salePriceQuantityString = "";
            for (int i = index_start + 1; i < index_end; i++)
            {
                salePriceQuantityString += saleRule[i];
            }
            buyMoreSaleInfo.salePriceQuantity = Convert.ToInt32(salePriceQuantityString);

            saleRule = saleRule.Substring(index_end + 1);
            index_start = saleRule.IndexOf("[");
            index_end = saleRule.IndexOf("]");
            string discountPercentString = "";
            for (int i = index_start + 1; i < index_end; i++)
            {
                discountPercentString += saleRule[i];
            }
            buyMoreSaleInfo.discountPercent = Convert.ToDecimal(discountPercentString.Substring(0, discountPercentString.Length - 1)) / 100.0m;            

            return buyMoreSaleInfo;
        }

    }
}
