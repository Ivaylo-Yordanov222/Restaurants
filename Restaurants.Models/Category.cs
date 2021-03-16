using Restaurants.Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restaurants.Models
{
    public class Category
    {
        public Category()
        {
            this.Products = new List<Product>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 4, ErrorMessage = BussinessLogicConstants.StringLengthOfCategory)]
        [RegularExpression(BussinessLogicConstants.ProductAndDescRegexString, ErrorMessage = BussinessLogicConstants.CategoryNameValidationRegexMessage)]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}