using System.Collections.Generic;
using TrendyolShoppingCart.Models;

namespace TrendyolShoppingCart.Business.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IShoppingCart
    {
        /// <summary>
        /// Add product to Shopping Cart with quantity
        /// </summary>
        /// <param name="product"></param>
        /// <param name="quantity"></param>
        void AddItem(Product product, int quantity);

        /// <summary>
        /// Products
        /// </summary>
        Dictionary<string, Product> Products { get; }

        /// <summary>
        /// Product Categories in Shopping Cart
        /// </summary>
        List<string> ProductCategories { get; }

        /// <summary>
        /// Apply Campaign Discounts on Shopping Cart
        /// </summary>
        /// <param name="discounts"></param>
        void ApplyDiscounts(params Campaign[] discounts);

        /// <summary>
        /// Apply Copuon on Shopping Cart
        /// </summary>
        /// <param name="coupon"></param>
        void ApplyCoupon(Coupon coupon);

        /// <summary>
        /// Get Total Amount After Discounts
        /// </summary>
        /// <returns></returns>
        double GetTotalAmountAfterDiscounts();

        /// <summary>
        /// Get Coupon Discount
        /// </summary>
        /// <returns></returns>
        double GetCouponDiscount();

        /// <summary>
        /// Get Campaign Discount
        /// </summary>
        /// <returns></returns>
        double GetCampaignDiscount();

        /// <summary>
        /// Get Delivery Cost
        /// </summary>
        /// <returns></returns>
        double GetDeliveryCost();

        /// <summary>
        /// Print results
        /// </summary>
        /// <returns></returns>
        string Print();

        int GetNumberOfDeliveries();
        int GetNumberOfProducts();

    }
}
