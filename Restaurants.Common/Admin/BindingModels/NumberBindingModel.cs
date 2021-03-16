using Restaurants.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Restaurants.Common.Admin.BindingModels
{
    public class NumberBindingModel
    {
        [Required(ErrorMessage = BussinessLogicConstants.RequiredNumber)]
        public int Number { get; set; }
    }
}
