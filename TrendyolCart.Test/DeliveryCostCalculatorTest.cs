using Moq;
using NUnit.Framework;
using TrendyolShoppingCart.Business;
using TrendyolShoppingCart.Business.Interfaces;
using TrendyolShoppingCart.Models;
using TrendyolShoppingCart.Models.enums;

namespace TrendyolCart.Test
{
    public class DeliveryCostCalculatorTest
    {
        DeliveryCostCalculator deliveryCostCalculator;
        Mock<ShoppingCart> shoppingCart;

        [SetUp]
        public void Setup()
        {
            shoppingCart = new Mock<ShoppingCart>();
        }

        [Test]
        public void Test_NoProducts()
        {
            deliveryCostCalculator = new DeliveryCostCalculator(3, 7);
            shoppingCart.Setup(m => m.GetNumberOfDeliveries()).Returns(0);
            shoppingCart.Setup(m => m.GetNumberOfProducts()).Returns(0);

            Assert.That(deliveryCostCalculator.CalculateFor(shoppingCart.Object) == 39);
        }

    }
}