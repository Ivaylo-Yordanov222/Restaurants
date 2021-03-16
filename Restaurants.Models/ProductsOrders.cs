using Restaurants.Common.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurants.Models
{
    public class ProductsOrders
    {
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = BussinessLogicConstants.QuantityRangeErrorMessage)]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(typeof(decimal), "0.01", "9999.99", ErrorMessage = BussinessLogicConstants.DecimalValidationMessage)]
        public decimal SoldPrice { get; set; }

        [Range(10, 90, ConvertValueInInvariantCulture = true)]
        public int Discount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(typeof(decimal), "0", "100.00", ErrorMessage = BussinessLogicConstants.DecimalValidationMessage)]
        public decimal ProductDiscount { get; set; }
    }
}