using Microsoft.AspNetCore.Identity;
using Restaurants.Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restaurants.Models
{
    public class User: IdentityUser
    {
        public User()
        {
            this.Orders = new List<Order>();
        }
        
        [RegularExpression(BussinessLogicConstants.UserNameRegexString, ErrorMessage = BussinessLogicConstants.ProductNameValidationRegex)]
        public string Fullname { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
