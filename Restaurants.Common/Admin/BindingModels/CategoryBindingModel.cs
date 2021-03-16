using Restaurants.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Restaurants.Common.Admin.BindingModels
{
    public class CategoryBindingModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = BussinessLogicConstants.RequiredField)]
        [StringLength(60, MinimumLength = 4, ErrorMessage = BussinessLogicConstants.StringLengthOfCategory)]
        [RegularExpression(BussinessLogicConstants.ProductAndDescRegexString, ErrorMessage = BussinessLogicConstants.CategoryNameValidationRegexMessage)]
        public string Name { get; set; }
    }
}
