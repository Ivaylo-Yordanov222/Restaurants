using Restaurants.Common.Enums;
using Restaurants.Common.Table.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Restaurants.Common.Table.BindingModels
{
    public class OrderBindingModel
    {
        public OrderBindingModel()
        {
            this.Products = new List<ProductViewModel>();
        }
        public int OrderId { get; set; }

        public string UserId { get; set; }

        public string Username { get; set; }

        public Status Status { get; set; }

        public DateTime StartTime { get; set; }

        public string DateAndTime { get; set; }

        public decimal Price { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
