using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace KioskCheckoutSystem.Tests
{
    [TestClass()]
    public class ReceiptTests
    {
        [TestMethod()]
        public void GetTotalPriceTest()
        {
            var singleItemReceipt1 = new SingleItemReceipt();
            var singleItemReceipt2 = new SingleItemReceipt();
            singleItemReceipt1.ProductName = "Apple";
            singleItemReceipt1.RegularPrice = 2;
            singleItemReceipt1.Saving = 0.5m;
            singleItemReceipt2.ProductName = "Orange";
            singleItemReceipt2.RegularPrice = 1;
            singleItemReceipt2.Saving = 0;

            var receipt = new Receipt();
            receipt.SingleItemReceiptList = new List<SingleItemReceipt>();
            receipt.SingleItemReceiptList.Add(singleItemReceipt1);
            receipt.SingleItemReceiptList.Add(singleItemReceipt2);
            var actualValue = 2.5m;

            Assert.AreEqual(receipt.TotalPrice, actualValue);

        }
    }
}