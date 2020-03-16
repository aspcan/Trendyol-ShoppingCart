namespace TrendyolShoppingCart.Models
{
    public class Product
    {
        public string Title { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public Category Category { get; set; }
        public double CalculatedTotalDiscount { get; set; }

        public double TotalPrice
        {
            get
            {
                return Price * Quantity - Category.CalculatedTotalDiscount;
            }
        }

        public Product(string title, double price, Category category)
        {
            Title = title;
            Price = price;
            Category = category;
        }

    }
}
