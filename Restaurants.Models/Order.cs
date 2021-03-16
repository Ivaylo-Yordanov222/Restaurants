using Restaurants.Common.Constants;
using Restaurants.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurants.Models
{
    public class Order
    {
        public Order()
        {
            this.ProductsInOrder = new List<ProductsOrders>();
        }
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        public ICollection<ProductsOrders> ProductsInOrder { get; set; }

        public Status Status { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(typeof(decimal), "0.01", "99999.99", ErrorMessage = BussinessLogicConstants.DecimalValidationMessage)]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(typeof(decimal), "0.01", "99999.99", ErrorMessage = BussinessLogicConstants.DecimalValidationMessage)]
        public decimal PromotionPrice { get; set; }

        [Range(10, 90, ConvertValueInInvariantCulture = true)]
        public int Discount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DeliverTime { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndTime { get; set; }
    }
}
