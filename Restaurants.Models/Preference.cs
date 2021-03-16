using Restaurants.Common.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Restaurants.Models
{
    public class Preference
    {
        public int Id { get; set; }

        [Required]
        [Range(5, 95, ErrorMessage = BussinessLogicConstants.DiscountValidationMessage)]
        public int Discount { get; set; }

        [Required(ErrorMessage = BussinessLogicConstants.MilisecondsToTakeDiscoutValidatingMessage)]
        [Display(Name = BussinessLogicConstants.MilisecondsToTakeDiscoutDisplayMessage)]
        public int MilisecondsToTakeDiscount { get; set; }

        [Required]
        [Display(Name = BussinessLogicConstants.DisplayItemsPerRowDisplayMessage)]
        [Range(3, 5, ErrorMessage = BussinessLogicConstants.DisplayItemsPerRowValidatingMessage)]
        public int DisplayItemsPerRow { get; set; }

        [Required]
        [Display(Name = BussinessLogicConstants.MaxNumberOfOrdersPerTableDisplayMessage)]
        [Range(1, 10, ErrorMessage = BussinessLogicConstants.MaxNumberOfOrdersPerTableValidatingMessage)]
        public int MaxNumberOfOrdersPerTable { get; set; }

        [Required]
        [Display(Name = BussinessLogicConstants.MaxNumberOfItemsInBagDisplayMessage)]
        [Range(10, 100, ErrorMessage = BussinessLogicConstants.MaxNumberOfItemsInBagValidatingMessage)]
        public int MaxNumberOfItemsInBag { get; set; }
    }
}
