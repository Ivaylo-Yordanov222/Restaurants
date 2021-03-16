using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restaurants.Common.Admin.ViewModels
{
    public class ProductSoldsViewModel
    {
        public ProductSoldsViewModel()
        {
            this.Solds = new List<ProductPricesViewModel>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string ImageUrl { get; set; }

        public string ImageTumbUrl { get; set; }

        public decimal TotalPrice 
        {
            get
            {
                return Solds.Sum(s => s.Total);
            }
        }

        public int TotalQuantity
        {
            get
            {
                return Solds.Sum(s => s.Quantity);
            }
        }

        public ICollection<ProductPricesViewModel> Solds { get; set; }
    }
}
