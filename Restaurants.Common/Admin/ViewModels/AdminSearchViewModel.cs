using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurants.Common.Admin.ViewModels
{
    public class AdminSearchViewModel
    {
        public AdminSearchViewModel()
        {
            this.Orders = new List<OrderConciseViewModel>();
            this.Users = new List<SelectListItem>();
        }
       
        public IEnumerable<OrderConciseViewModel> Orders { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }
    }
}
