using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurants.Common.Admin.ViewModels
{
    public class OrderConciseViewModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public decimal Price { get; set; }

        public decimal? PromotionPrice { get; set; }

        public int? Discount { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime DeliverTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
