using System.Collections.Generic;

namespace Restaurants.Common.Admin.ViewModels
{
    public class CategoryDetailsViewModel
    {
        public CategoryDetailsViewModel()
        {
            this.Products = new List<ProductConciseViewModel>();
            this.Categories = new List<CategoryConciseViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProductConciseViewModel> Products { get; set; }

        public IEnumerable<CategoryConciseViewModel> Categories { get; set; }
    }
}
