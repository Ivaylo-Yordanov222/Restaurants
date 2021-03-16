namespace Restaurants.Common.Table.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public int Quantity { get; set; } = 1;

        public string Name { get; set; }

        public string Slug { get; set; }

        public string ImageUrl { get; set; }

        public string ImageTumbUrl { get; set; }

        public decimal Price { get; set; }

        public decimal PromotionalPrice { get; set; }

        public decimal Discount { get; set; }

        public decimal Total
        {
            get
            {
                if (this.PromotionalPrice == 0m)
                {
                    return this.Price * this.Quantity;
                }
                else
                {
                    return this.PromotionalPrice * this.Quantity;
                }
            }
        }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}
