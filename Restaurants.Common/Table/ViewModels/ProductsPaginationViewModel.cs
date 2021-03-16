using Restaurants.Common.Admin.ViewModels;
using System.Collections.Generic;

namespace Restaurants.Common.Table.ViewModels
{
    public class ProductsPaginationViewModel
    {
        public int Page { get; set; }

        public string CategoryName { get; set; }

        public int NumberOfPages { get; set; }

        public IEnumerable<CategoryConciseViewModel> Categories { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
