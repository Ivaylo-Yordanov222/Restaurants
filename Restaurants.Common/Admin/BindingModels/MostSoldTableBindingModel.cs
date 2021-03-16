using System;
using System.ComponentModel.DataAnnotations;
using Restaurants.Common.Constants;
using Restaurants.Common.Utilities;

namespace Restaurants.Common.Admin.BindingModels
{
    public class MostSoldTableBindingModel
    {
        [Required(ErrorMessage = BussinessLogicConstants.RequiredStartDateMessage)]
        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = BussinessLogicConstants.RequiredEndDateMessage)]
        [DateGreaterThan(otherPropertyName = "StartTime", ErrorMessage = BussinessLogicConstants.EndDateMustBeAfteStartDateMessage)]
        [DataType(DataType.Date)]
        public DateTime EndTime { get; set; }
    }
}
