namespace TrendyolShoppingCart.Models
{
    public class Category
    {
        public Category ParentCategory { get; private set; }
        public string Title { get; private set; }
        public double CalculatedTotalDiscount { get; set; }

        public Category(string title)
        {
            Title = title;
        }

        public Category(string title, Category parentCategory)
        {
            Title = title;
            ParentCategory = parentCategory;
        }
    }
}
