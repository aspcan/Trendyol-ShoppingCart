using TrendyolShoppingCart.Business.Interfaces;

namespace TrendyolShoppingCart.Business
{
    public class DeliveryCostCalculator : IDeliveryCostCalculator
    {
        private const double FixedCost = 3.5;
        private double CostPerDelivery;
        private double CostPerProduct;
        public DeliveryCostCalculator(double costPerDelivery, double costPerProduct)
        {
            CostPerDelivery = costPerDelivery;
            CostPerProduct = costPerProduct;
        }

        public double CalculateFor(ShoppingCart cart)
        {
            int numberOfDeliveries = cart.ProductCategories.Count;
            int numberOfProducts = cart.Products.Count;
            double deliveryCost = (CostPerDelivery * numberOfDeliveries) + (CostPerProduct * numberOfProducts) + FixedCost;
            return deliveryCost;
        }
    }
}
