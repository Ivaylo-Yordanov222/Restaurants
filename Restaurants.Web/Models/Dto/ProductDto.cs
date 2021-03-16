
using System.ComponentModel.DataAnnotations;

namespace Restaurants.Web.Models.Dto
{
    public class ProductDto
    {
        [Required]
        [StringLength(70, MinimumLength = 3, ErrorMessage = "The product name must have min length of 3 and max length of 70 characters!")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^(([a-z]+)(-)([a-z]+))*$")]
        public string Slug { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "Product description must be min 3 characters and max 500 characters of length!")]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string ImageTumbUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [Range(typeof(decimal), "0", "9999", ErrorMessage = "{0} must be a decimal/number between {1} and {2}.")]
        public decimal Price { get; set; }
    }
}
