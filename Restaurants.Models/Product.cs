using Restaurants.Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurants.Models
{
    public class Product
    {
        public Product()
        {
            this.OrdersWithProduct = new List<ProductsOrders>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 4, ErrorMessage = BussinessLogicConstants.StringLengthOfProduct)]
        [RegularExpression(BussinessLogicConstants.ProductAndDescRegexString, ErrorMessage = BussinessLogicConstants.ProductNameValidationRegex)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(BussinessLogicConstants.SlugRegexString, ErrorMessage = BussinessLogicConstants.SlugValidationRegex)]
        public string Slug { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 3, ErrorMessage = BussinessLogicConstants.StringLengthOfDescription)]
        [RegularExpression(BussinessLogicConstants.ProductAndDescRegexString, ErrorMessage = BussinessLogicConstants.ProductNameValidationRegex)]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string ImageTumbUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [Required(ErrorMessage = BussinessLogicConstants.RequiredCustomDecimal)]
        [Column(TypeName = "decimal(18,2)")]
        [Range(typeof(decimal), "0.01", "9999.99", ErrorMessage = BussinessLogicConstants.DecimalValidationMessage)]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(typeof(decimal), "0", "9999.99", ErrorMessage = BussinessLogicConstants.DecimalValidationMessage)]
        public decimal PromotionalPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(typeof(decimal), "0", "100.00", ErrorMessage = BussinessLogicConstants.DecimalValidationMessage)]
        public decimal Discount { get; set; }

        public ICollection<ProductsOrders> OrdersWithProduct { get; set; }
    }
}
