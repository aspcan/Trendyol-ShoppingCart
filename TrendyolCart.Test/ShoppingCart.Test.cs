using Moq;
using NUnit.Framework;
using TrendyolShoppingCart.Business;
using TrendyolShoppingCart.Business.Interfaces;
using TrendyolShoppingCart.Models;
using TrendyolShoppingCart.Models.enums;

namespace TrendyolCart.Test
{
    public class ShoppingCartTest
    {
        Mock<IDeliveryCostCalculator> shoppingCalculator;
        IShoppingCart shoppingCart;

        [SetUp]
        public void Setup()
        {
            shoppingCalculator = new Mock<IDeliveryCostCalculator>();
            shoppingCart = new ShoppingCart(shoppingCalculator.Object);
        }






        [Test]
        public void AddProduct_TestProductQuantity()
        {
            var electronic = new Category("Electronic");
            var computer = new Product("Lenovo Thinkpad X1", 1400, electronic);

            shoppingCart.AddItem(computer, 8);
            shoppingCart.AddItem(computer, 8);
            Assert.AreEqual(16, shoppingCart.Products[computer.Title].Quantity);
            Assert.AreEqual(1, shoppingCart.Products.Count);
        }

        [Test]
        public void AddProduct_Compaigndiscount()
        {
            var electronic = new Category("Electronic");
            var computer = new Product("Lenovo Thinkpad X1", 1000, electronic);

            var campaign = new Campaign(electronic, 20, 2, DiscountType.Rate);
            shoppingCart.ApplyDiscounts(campaign);

            shoppingCart.AddItem(computer, 5);
            shoppingCart.AddItem(computer, 5);

            double totalAmount = shoppingCart.GetTotalAmountAfterDiscounts();
            Assert.AreEqual(8000, totalAmount);
        }
    }
}