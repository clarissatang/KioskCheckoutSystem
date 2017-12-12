
namespace KioskCheckoutSystem
{
    public class ProductModel
    {
        public string Name { get; set; }
        public decimal RegularPrice { get; set; }
        public bool IsOnSale { get; set; }
        public decimal SalePrice { get; set; }
        public bool IsAdditionalSale { get; set; }
        public string SaleRule { get; set; }
    }
}
