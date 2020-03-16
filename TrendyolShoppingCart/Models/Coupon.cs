using TrendyolShoppingCart.Models.enums;

namespace TrendyolShoppingCart.Models
{
    public class Coupon
    {
        public double MinimumCartAmount { get; private set; }
        public int DiscountValue { get; private set; }
        public DiscountType DiscountType { get; private set; }

        public Coupon(double minimumCartAmount, int discountValue, DiscountType discountType)
        {
            MinimumCartAmount = minimumCartAmount;
            DiscountValue = discountValue;
            DiscountType = discountType;
        }
    }
}
