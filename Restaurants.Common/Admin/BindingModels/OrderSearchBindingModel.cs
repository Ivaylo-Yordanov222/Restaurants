using System;
using Restaurants.Common.Utilities;
using System.ComponentModel.DataAnnotations;
using Restaurants.Common.Constants;
using Microsoft.Extensions.Localization;
using Restaurants.Common.Resources;

namespace Restaurants.Common.Admin.BindingModels
{
    public class OrderSearchBindingModel
    {
        [Required(ErrorMessage = BussinessLogicConstants.RequiredUser)]
        public string UserId { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = BussinessLogicConstants.RequiredStartDateMessage)]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = BussinessLogicConstants.RequiredEndDateMessage)]
        [DateGreaterThan(otherPropertyName = "StartTime", ErrorMessage = BussinessLogicConstants.EndDateMustBeAfteStartDateMessage)]
        [DataType(DataType.Date)]
        public DateTime EndTime { get; set; }
    }
}
