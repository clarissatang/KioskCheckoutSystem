/*****************************************************************
Filename:       Promotion.cs
Revised:        Date: 2017/02/23
Revision:       Revision: 1.0.0

Description:    Get the product price based on different promotion rule

Revision log:
* 2017-02-21: Created
******************************************************************/
using System;
using System.Collections.Generic;

namespace KioskCheckoutSystem
{
    public class Promotion
    {
        private const string GROUPSALE = "Group";
        private const string BUYMORESALE = "Buy More";

        private readonly ProductModel _thisProductData;

        public Promotion(ProductModel thisProductData)
        {
            _thisProductData = thisProductData;
        }

        public List<SingleItemReceipt> OnSaleItemReceipt(int quantity)
        {
            try
            {
                var isOnAdditionalSale = _thisProductData.IsAdditionalSale;
                var groupOneItemReceipt = new List<SingleItemReceipt>();

                if (isOnAdditionalSale)
                {
                    var saleRule = _thisProductData.SaleRule;

                    if (saleRule.StartsWith(BUYMORESALE))
                    {
                        var buyMoreSaleInfo = GetBuyMoreSaleDetail(saleRule);
                        groupOneItemReceipt = GetSaleRuleReceipt(quantity, buyMoreSaleInfo);
                    }
                    else if (saleRule.StartsWith(GROUPSALE))
                    {
                        var groupSaleInfo = GetGroupSaleDetail(saleRule);
                        groupOneItemReceipt = GetSaleRuleReceipt(quantity, groupSaleInfo);

                    }
                }
                else // regular sale
                {
                    groupOneItemReceipt = GetSaleRuleReceipt(quantity);
                }

                return groupOneItemReceipt;
            }
            catch (Exception ex)
            {
                CollectError.CollectErrorToFile(ex, Program.ErrorFile);
                return null;
            }
        }

        // Get the receipt detail for the regular sale item.
        private List<SingleItemReceipt> GetSaleRuleReceipt(int quantity)
        {
            try
            {
                var groupOneItemReceipt = new List<SingleItemReceipt>();
                for (var i = 0; i < quantity; i++)
                {
                    var oneItemReceipt = new SingleItemReceipt
                    {
                        ProductName = _thisProductData.Name,
                        RegularPrice = _thisProductData.RegularPrice,
                        Saving = _thisProductData.RegularPrice - _thisProductData.SalePrice
                    };
                    groupOneItemReceipt.Add(oneItemReceipt);
                }
                return groupOneItemReceipt;
            }
            catch (Exception ex)
            {
                CollectError.CollectErrorToFile(ex, Program.ErrorFile);
                return null;
            }
        }

        // Get the receipt detail for the buy more sale item, but 2 get 1 50% off.
        private List<SingleItemReceipt> GetSaleRuleReceipt(int quantity, BuyMoreSale buyMoreSale)
        {
            try
            {
                var productName = _thisProductData.Name;
                var regularPrice = _thisProductData.RegularPrice;
                var groupOneItemReceipt = new List<SingleItemReceipt>();
                var groupNumber = quantity / (buyMoreSale.RegularPriceQuantity + buyMoreSale.SalePriceQuantity);
                var outofGroupQuantity = quantity % (buyMoreSale.RegularPriceQuantity + buyMoreSale.SalePriceQuantity);

                var quo = outofGroupQuantity / buyMoreSale.RegularPriceQuantity;
                var rem = outofGroupQuantity % buyMoreSale.RegularPriceQuantity;

                var regularPriceNumber = quo == 0
                    ? groupNumber * buyMoreSale.RegularPriceQuantity + rem
                    : (groupNumber + quo) * buyMoreSale.RegularPriceQuantity;
                
                var salePriceNumber = quantity - regularPriceNumber;

                for (var i = 0; i < regularPriceNumber; i++)
                {
                    var oneItemReceipt = new SingleItemReceipt
                    {
                        ProductName = productName,
                        RegularPrice = regularPrice,
                        Saving = 0
                    };
                    groupOneItemReceipt.Add(oneItemReceipt);
                }

                for (var i = 0; i < salePriceNumber; i++)
                {
                    var oneItemReceipt = new SingleItemReceipt
                    {
                        ProductName = productName,
                        RegularPrice = regularPrice,
                        Saving = buyMoreSale.DiscountPercent == 1
                            ? regularPrice
                            : Math.Round(regularPrice * buyMoreSale.DiscountPercent, 2)
                    };
                    groupOneItemReceipt.Add(oneItemReceipt);
                }

                return groupOneItemReceipt;
            }
            catch (Exception ex)
            {
                CollectError.CollectErrorToFile(ex, Program.ErrorFile);
                return null;
            }
        }

        // Get the receipt detail for buy more sale item, buy 3 get 1 50% off
        private List<SingleItemReceipt> GetSaleRuleReceipt(int quantity, GroupSale groupSale)
        {
            try
            {
                var productName = _thisProductData.Name;
                var regularPrice = _thisProductData.RegularPrice;
                var groupOneItemReceipt = new List<SingleItemReceipt>();
                var groupNumber = quantity / groupSale.GroupQuantity;
                var outOfGroupQuantity = quantity % groupSale.GroupQuantity;
                for (var i = 0; i < groupNumber; i++) // use group price
                {
                    var priceTotalTmp = 0.0m;
                    for (var j = 0; j < groupSale.GroupQuantity; j++)
                    {
                        var oneItemReceipt = new SingleItemReceipt
                        {
                            ProductName = productName,
                            RegularPrice = regularPrice
                        };
                        decimal salePrice;

                        if (j == groupSale.GroupQuantity - 1) // this is the last one, price might be different, e.g., buy 3 for $2
                        {
                            salePrice = groupSale.GroupPrice - priceTotalTmp;
                        }
                        else
                        {
                            salePrice = Math.Round(groupSale.GroupPrice / groupSale.GroupQuantity, 2);
                            priceTotalTmp += salePrice;
                        }
                        oneItemReceipt.Saving = regularPrice - salePrice;
                        groupOneItemReceipt.Add(oneItemReceipt);
                    }
                }

                for (var i = 0; i < outOfGroupQuantity; i++) // use regular price
                {
                    var oneItemReceipt = new SingleItemReceipt
                    {
                        ProductName = productName,
                        RegularPrice = regularPrice,
                        Saving = 0
                    };
                    groupOneItemReceipt.Add(oneItemReceipt);
                }
                return groupOneItemReceipt;
            }
            catch (Exception ex)
            {
                CollectError.CollectErrorToFile(ex, Program.ErrorFile);
                return null;
            }
        }
        

        // get the parameters for group sale
        private static GroupSale GetGroupSaleDetail(string saleRule)
        {
            var groupSale = new GroupSale();
            try
            {                
                // format in database: Group: buy [3] for $[2]
                var indexStart = saleRule.IndexOf("[", StringComparison.InvariantCulture);
                var indexEnd = saleRule.IndexOf("]", StringComparison.InvariantCulture);
                var groupQuantityString = "";
                for (var i = indexStart + 1; i < indexEnd; i++)
                {
                    groupQuantityString += saleRule[i];
                }
                groupSale.GroupQuantity = Convert.ToInt32(groupQuantityString);

                saleRule = saleRule.Substring(indexEnd + 1);
                indexStart = saleRule.IndexOf("[", StringComparison.InvariantCulture);
                indexEnd = saleRule.IndexOf("]", StringComparison.InvariantCulture);
                var groupPriceString = "";
                for (var i = indexStart + 1; i < indexEnd; i++)
                {
                    groupPriceString += saleRule[i];
                }
                groupSale.GroupPrice = Convert.ToDecimal(groupPriceString);
                
            }
            catch (Exception ex)
            {
                CollectError.CollectErrorToFile(ex, Program.ErrorFile);                
            }
            return groupSale;
        }

        // get the parameters for buy more sale
        private static BuyMoreSale GetBuyMoreSaleDetail(string saleRule)
        {
            var buyMoreSaleInfo = new BuyMoreSale();
            try
            {
                // format in database: Additional: buy [1] get [1] [50%] off
                var indexStart = saleRule.IndexOf("[", StringComparison.InvariantCulture);
                var indexEnd = saleRule.IndexOf("]", StringComparison.InvariantCulture);
                var regularPriceQuantityString = "";
                for (var i = indexStart + 1; i < indexEnd; i++)
                {
                    regularPriceQuantityString += saleRule[i];
                }
                buyMoreSaleInfo.RegularPriceQuantity = Convert.ToInt32(regularPriceQuantityString);

                saleRule = saleRule.Substring(indexEnd + 1);
                indexStart = saleRule.IndexOf("[", StringComparison.InvariantCulture);
                indexEnd = saleRule.IndexOf("]", StringComparison.InvariantCulture);
                var salePriceQuantityString = "";
                for (var i = indexStart + 1; i < indexEnd; i++)
                {
                    salePriceQuantityString += saleRule[i];
                }
                buyMoreSaleInfo.SalePriceQuantity = Convert.ToInt32(salePriceQuantityString);

                saleRule = saleRule.Substring(indexEnd + 1);
                indexStart = saleRule.IndexOf("[", StringComparison.InvariantCulture);
                indexEnd = saleRule.IndexOf("]", StringComparison.InvariantCulture);
                var discountPercentString = "";
                for (var i = indexStart + 1; i < indexEnd; i++)
                {
                    discountPercentString += saleRule[i];
                }
                buyMoreSaleInfo.DiscountPercent = Convert.ToDecimal(discountPercentString.Substring(0, discountPercentString.Length - 1)) / 100.0m;
            }
            catch (Exception ex)
            {
                CollectError.CollectErrorToFile(ex, Program.ErrorFile);
            }
            return buyMoreSaleInfo;
        }

    }
}
