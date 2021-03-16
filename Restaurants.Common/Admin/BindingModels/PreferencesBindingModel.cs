using System.ComponentModel.DataAnnotations;
using Restaurants.Common.Constants;

namespace Restaurants.Common.Admin.BindingModels
{
    public class PreferencesBindingModel
    {
        [Required(ErrorMessage = BussinessLogicConstants.RequiredField)]
        [Range(5, 95, ErrorMessage = BussinessLogicConstants.DiscountValidationMessage)]
        [Display(Name = BussinessLogicConstants.DiscountDisplayMessage)]
        public int Discount { get; set; }

        [Required(ErrorMessage = BussinessLogicConstants.MilisecondsToTakeDiscoutValidatingMessage)]
        [Display(Name = BussinessLogicConstants.MilisecondsToTakeDiscoutDisplayMessage)]
        public int MilisecondsToTakeDiscount { get; set; }

        [Required(ErrorMessage = BussinessLogicConstants.RequiredField)]
        [Display(Name = BussinessLogicConstants.DisplayItemsPerRowDisplayMessage)]
        [Range(3, 5, ErrorMessage = BussinessLogicConstants.DisplayItemsPerRowValidatingMessage)]
        public int DisplayItemsPerRow { get; set; }

        [Required(ErrorMessage = BussinessLogicConstants.RequiredField)]
        [Display(Name = BussinessLogicConstants.MaxNumberOfOrdersPerTableDisplayMessage)]
        [Range(1, 10, ErrorMessage = BussinessLogicConstants.MaxNumberOfOrdersPerTableValidatingMessage)]
        public int MaxNumberOfOrdersPerTable { get; set; }

        [Required(ErrorMessage = BussinessLogicConstants.RequiredField)]
        [Display(Name = BussinessLogicConstants.MaxNumberOfItemsInBagDisplayMessage)]
        [Range(10, 100, ErrorMessage = BussinessLogicConstants.MaxNumberOfItemsInBagValidatingMessage)]
        public int MaxNumberOfItemsInBag { get; set; }
    }
}
