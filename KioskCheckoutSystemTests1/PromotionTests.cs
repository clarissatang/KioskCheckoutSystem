using Microsoft.VisualStudio.TestTools.UnitTesting;
using KioskCheckoutSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KioskCheckoutSystem.Data;

namespace KioskCheckoutSystem.Tests
{
    [TestClass()]
    public class PromotionTests
    {
        const string BuyMoreSaleItemFile = "BuyMoreSaleItem.txt"; // buy 1 get 1 40% off, regular 1.59
        const string GroupSaleItemFile = "GroupSaleItem.txt"; // buy 3 for $2, regular 1.59
        //const string NotSaleItemFile = "NotSaleItem.txt";
        //const string RegularSaleItem = "RegularSaleItem.txt";

        private IteamDataBase GetOneItemDataBase(string testFile)
        {
            string[] data = File.ReadAllLines(testFile);
            IteamDataBase oneItemDataBase = new IteamDataBase();
            oneItemDataBase.ItemDataEntry = new string[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                oneItemDataBase.ItemDataEntry[i] = data[i];
            }
            return oneItemDataBase;
        }
        
        [TestMethod()]
        public void GroupSaleTest()
        {
            IteamDataBase oneItemDataBase = GetOneItemDataBase(GroupSaleItemFile);
            Promotion promotion = new Promotion();
            List<SingleItemReceipt> oneProductReceipt = promotion.OnSaleItem(5, oneItemDataBase);
            Receipt receipt = new Receipt();
            receipt.singleItemReceiptList = oneProductReceipt;
            decimal actualValue = 2 + 1.59m + 1.59m;
            Assert.AreEqual(receipt.TotalPrice, actualValue);
        }

        [TestMethod()]
        public void BuyMoreSaleTest()
        {
            IteamDataBase oneItemDataBase = GetOneItemDataBase(BuyMoreSaleItemFile);
            Promotion promotion = new Promotion();
            List<SingleItemReceipt> oneProductReceipt = promotion.OnSaleItem(5, oneItemDataBase);
            Receipt receipt = new Receipt();
            receipt.singleItemReceiptList = oneProductReceipt;
            decimal actualValue = 1.59m*3 + (1.59m-Math.Round(1.59m*0.4m, 2)) *2;
            Assert.AreEqual(receipt.TotalPrice, actualValue);
        }

    }
}