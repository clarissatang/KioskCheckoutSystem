using Microsoft.VisualStudio.TestTools.UnitTesting;
using KioskCheckoutSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskCheckoutSystem.Tests
{
    [TestClass()]
    public class ReceiptTests
    {
        [TestMethod()]
        public void GetTotalPriceTest()
        {
            SingleItemReceipt singleItemReceipt1 = new SingleItemReceipt();
            SingleItemReceipt singleItemReceipt2 = new SingleItemReceipt();
            singleItemReceipt1.ProductName = "Apple";
            singleItemReceipt1.RegularPrice = 2;
            singleItemReceipt1.Saving = 0.5m;
            singleItemReceipt2.ProductName = "Orange";
            singleItemReceipt2.RegularPrice = 1;
            singleItemReceipt2.Saving = 0;

            Receipt receipt = new Receipt();
            receipt.singleItemReceiptList = new List<SingleItemReceipt>();
            receipt.singleItemReceiptList.Add(singleItemReceipt1);
            receipt.singleItemReceiptList.Add(singleItemReceipt2);
            decimal actualValue = 2.5m;

            Assert.AreEqual(receipt.TotalPrice, actualValue);

        }
    }
}