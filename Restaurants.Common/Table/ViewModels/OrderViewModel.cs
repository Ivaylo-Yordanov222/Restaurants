using Restaurants.Common.Enums;
using System;
using System.Collections.Generic;

namespace Restaurants.Common.Table.ViewModels
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {
            this.Products = new List<ProductViewModel>();
        }
        public int OrderId { get; set; }

        public string Username { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? DeliverTime { get; set; }

        public decimal Price { get; set; }

        public decimal PromotionPrice { get; set; }

        public Status Status { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
