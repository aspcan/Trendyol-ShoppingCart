using System;
using TrendyolShoppingCart.Business;
using TrendyolShoppingCart.Business.Interfaces;
using TrendyolShoppingCart.Models;
using TrendyolShoppingCart.Models.enums;

namespace TrendyolCart
{
    class Program
    {
        static void Main(string[] args)
        {
            IShoppingCart shoppingCart = new ShoppingCart(new DeliveryCostCalculator(3.5, 5));

            var electronic = new Category("Electronic");
            var accessory = new Category("Accessory");
            var phone = new Category("Phone", electronic);

            var computer = new Product("Lenovo Thinkpad X1", 1400, electronic);
            var computer2 = new Product("Dell XPS", 1500, electronic);
            var computer3 = new Product("Apple Mac Pro", 1950, electronic);
            var watch = new Product("Rolex iwatch 10", 300, accessory);
            var watch2 = new Product("Casio WR", 150, accessory);
            var iPhone = new Product("IPhone 11 Pro 512 GB", 1200, phone);

            shoppingCart.AddItem(computer, 8);
            shoppingCart.AddItem(computer2, 2);
            shoppingCart.AddItem(computer3, 5);
            shoppingCart.AddItem(watch, 2);
            shoppingCart.AddItem(iPhone, 4);

            var campaign1 = new Campaign(electronic, 20, 2, DiscountType.Rate);
            var campaign2 = new Campaign(accessory, 500, 1, DiscountType.Amount);

            shoppingCart.ApplyDiscounts(campaign1, campaign2);

            var coupon = new Coupon(1000, 10, DiscountType.Rate);
            shoppingCart.ApplyCoupon(coupon);


            double totalAmountAfterDiscounts = shoppingCart.GetTotalAmountAfterDiscounts();
            double campaignDiscount = shoppingCart.GetCampaignDiscount();
            double deliveryCost = shoppingCart.GetDeliveryCost();
            double couponDiscount = shoppingCart.GetCouponDiscount();

            System.Console.WriteLine(shoppingCart.Print());
            System.Console.Read();
        }
    }
}
