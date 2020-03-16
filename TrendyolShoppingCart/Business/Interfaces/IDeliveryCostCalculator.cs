namespace TrendyolShoppingCart.Business.Interfaces
{
    public interface IDeliveryCostCalculator
    {
        double CalculateFor(ShoppingCart cart);
    }
}
