using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TrendyolShoppingCart.Business.Interfaces;
using TrendyolShoppingCart.Models;

namespace TrendyolShoppingCart.Business
{
    public class ShoppingCart : IShoppingCart
    {

        private IDeliveryCostCalculator DeliveryCostCalculator { get; set; }
        public Dictionary<string, Product> Products { get; set; }
        List<Campaign> campaigns;
        Coupon coupon;
        double cartTotalAmount = 0;
        double cartTotalAmountAfterDiscounts = 0;
        double campaignDiscount = 0;
        double couponDiscount = 0;

        public ShoppingCart(IDeliveryCostCalculator deliveryCostCalculator)
        {
            DeliveryCostCalculator = deliveryCostCalculator;
        }


        public List<string> ProductCategories
        {
            get
            {
                if (this.Products != null && this.Products.Count > 0)
                {
                    return GetProductCategoriesInCart();
                }
                return null;
            }
        }

        public void AddItem(Product product, int quantity)
        {
            if (quantity < 1)
                return;

            if (String.IsNullOrEmpty(product.Title))
                return;

            if (Products == null)
            {
                Products = new Dictionary<string, Product>();
            }

            if (!Products.ContainsKey(product.Title))
            {
                product.Quantity = quantity;
                Products.Add(product.Title, product);
            }
            else
            {
                Products[product.Title].Quantity += quantity;
            }
            cartTotalAmount += product.Price * quantity;
        }

        public void ApplyDiscounts(params Campaign[] discounts)
        {
            campaigns = new List<Campaign>();
            campaigns.AddRange(discounts);
        }

        public void ApplyCoupon(Coupon coupon)
        {
            this.coupon = coupon;
        }

        public double GetTotalAmountAfterDiscounts()
        {
            ApplyCampaignsDiscounts();
            ApplyCouponDiscount();
            return cartTotalAmountAfterDiscounts;
        }

        public double GetCouponDiscount()
        {
            return couponDiscount;
        }

        public double GetCampaignDiscount()
        {
            return campaignDiscount;
        }

        public double GetDeliveryCost()
        {
            return DeliveryCostCalculator.CalculateFor(this);
        }

        public int GetNumberOfDeliveries()
        {
            return Products.GroupBy(e => e.Value.Title).Count();
        }
        public int GetNumberOfProducts()
        {
            return Products.Count;
        }
        public string Print()
        {
            var res = Products.GroupBy(x => x.Value.Category.Title).ToDictionary(key => key.Key, products => products.ToList());
            var result = Products.GroupBy(x => x.Value.Category).ToList();
            var res2 = Products.GroupBy(x => x.Value.Category.Title, (key, products) => new { CategoryTitle = key, product = products.ToList() });
            var productsByCategory = new StringBuilder();
            foreach (var category in res)
            {
                productsByCategory.Append($"*~*~*~*~*~*~* Category: {category.Key} ~*~*~*~*~*~*~*\n");

                foreach (var product in category.Value)
                {
                    productsByCategory.Append($" Category: {category.Key}\n");
                    productsByCategory.Append($" Product Name: {product.Value.Title}\n");
                    productsByCategory.Append($" Quantity: {product.Value.Quantity}\n");
                    productsByCategory.Append($" Unit Price: {product.Value.Price}\n");
                    productsByCategory.Append($" Total Price: {product.Value.TotalPrice}\n");
                    productsByCategory.Append($" Total Discount(coupon,campaign): {couponDiscount + product.Value.Category.CalculatedTotalDiscount}\n");
                    productsByCategory.Append("~~~~~~~~~~~~~~~~~~~~~~~");
                    productsByCategory.Append("\n");
                    productsByCategory.Append("\n");
                }
            }

            string totalAmount = $"Total Amount: {cartTotalAmount}\n";
            string deliveryCost = $"Delivery Cost: {GetDeliveryCost()}\n";
            productsByCategory.Append(totalAmount);
            productsByCategory.Append(deliveryCost);
            return productsByCategory.ToString();
        }

        private void ApplyCampaignsDiscounts()
        {
            cartTotalAmountAfterDiscounts = cartTotalAmount;
            double maxDiscountAmount = 0;
            double calculatedDiscount = 0;
            Dictionary<string, Product> discountedProducts = null;
            foreach (Campaign campaign in campaigns)
            {
                Dictionary<string, Product> productsInCategory = GetProductsInCategory(campaign.ProductCategory);
                int totalQuantityInCategory = productsInCategory.Values.Sum(x => x.Quantity);
                double totalPriceInCategory = productsInCategory.Values.Sum(x => x.Price * x.Quantity);

                if (totalQuantityInCategory > campaign.MinProductQuantity)
                {
                    switch (campaign.DiscountType)
                    {
                        case Models.enums.DiscountType.Rate:
                            calculatedDiscount = totalPriceInCategory * campaign.DiscountValue / 100;
                            if (calculatedDiscount > maxDiscountAmount)
                            {
                                maxDiscountAmount = calculatedDiscount;
                                discountedProducts = productsInCategory;
                            }

                            break;
                        case Models.enums.DiscountType.Amount:
                            if (campaign.DiscountValue > maxDiscountAmount)
                            {
                                maxDiscountAmount = campaign.DiscountValue;
                                //discountedCategory = campaign.ProductCategory;
                                discountedProducts = productsInCategory;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            SetDiscountedProductCategories(discountedProducts, maxDiscountAmount);
            campaignDiscount = maxDiscountAmount;
            cartTotalAmountAfterDiscounts -= maxDiscountAmount;

        }

        private Dictionary<string, Product> GetProductsInCategory(Category campaignCategory)
        {
            return Products.Where(x => x.Value.Category.Title == campaignCategory.Title ||
                                    HasSubCategory(campaignCategory, x.Value.Category)).
                                    ToDictionary(x => x.Key, x => x.Value);
        }

        private bool HasSubCategory(Category campaignCategory, Category productCategory)
        {
            if (productCategory == null || campaignCategory == null)
                return false;

            if (productCategory.ParentCategory == null)
                return false;
            if (productCategory.ParentCategory.Title == campaignCategory.Title)
            {
                return true;
            }

            if (campaignCategory.ParentCategory == null)
            {
                return false;
            }
            if (campaignCategory.ParentCategory.Title == productCategory.Title)
            {
                return true;
            }

            return false;
        }

        private void SetDiscountedProductCategories(Dictionary<string, Product> discountedProducts, double discountAmount)
        {
            if (discountedProducts == null || discountedProducts.Count < 1)
                return;

            discountedProducts.Values.ToList().ForEach(x => x.Category.CalculatedTotalDiscount = discountAmount);
        }

        private void ApplyCouponDiscount()
        {
            if (coupon == null)
                return;
            double calculatedDiscount = 0;
            cartTotalAmountAfterDiscounts = cartTotalAmount;

            if (cartTotalAmount >= coupon.MinimumCartAmount)
            {
                switch (coupon.DiscountType)
                {
                    case Models.enums.DiscountType.Rate:
                        calculatedDiscount = cartTotalAmount * coupon.DiscountValue / 100;
                        break;
                    case Models.enums.DiscountType.Amount:
                        calculatedDiscount = coupon.DiscountValue;
                        break;
                    default:
                        break;
                }
            }
            couponDiscount = calculatedDiscount;
            cartTotalAmountAfterDiscounts -= couponDiscount;
        }

        private List<string> GetProductCategoriesInCart()
        {
            return Products.Values.Select(x => x.Category.Title).Distinct().ToList();
        }

    }
}
