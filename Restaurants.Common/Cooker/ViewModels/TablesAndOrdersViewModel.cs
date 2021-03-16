using Restaurants.Common.Table.ViewModels;
using System.Collections.Generic;

namespace Restaurants.Common.Cooker.ViewModels
{
    public class TablesAndOrdersViewModel
    {
        public TablesAndOrdersViewModel()
        {
            this.Orders = new List<OrderViewModel>();
        }
        public string TableName { get; set; }

        public string TableId { get; set; }

        public string TableRole { get; set; }

        public IEnumerable<OrderViewModel> Orders { get; set; }
    }
}
