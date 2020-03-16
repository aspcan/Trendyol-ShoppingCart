using TrendyolShoppingCart.Models.enums;

namespace TrendyolShoppingCart.Models
{
    public class Campaign
    {
        public Category ProductCategory { get; private set; }
        public double DiscountValue { get; private set; }
        public int MinProductQuantity { get; private set; }
        public DiscountType DiscountType { get; private set; }

        public Campaign(Category productCategory, double discountValue, int minProductQuantity, DiscountType discountType)
        {
            this.ProductCategory = productCategory;
            this.DiscountValue = discountValue;
            this.MinProductQuantity = minProductQuantity;
            this.DiscountType = discountType;
        }
        
    }
}
