using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskCheckoutSystem.Data
{
    public struct Order
    {
        public string ProductName;
        public int Quantity;
    };

    public enum EnumItemData
    {
        ProductName,
        RegularPrice,
        isOnSale,
        SalePrice,
        isAdditionalSale,
        SaleRule
    };

    public struct IteamDataBase
    {
        public string[] ItemDataEntry;
    };
}
