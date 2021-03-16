namespace Restaurants.Common.Admin.ViewModels
{
    public class ProductConciseViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string ImageUrl { get; set; }

        public string ImageTumbUrl { get; set; }

        public decimal Price  { get; set; }

        public decimal PromotionalPrice { get; set; }

        public decimal Discount { get; set; }
    }
}
