using Restaurants.Common.Utilities;
using Restaurants.Common.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Restaurants.Common.Admin.BindingModels
{
    public class MostSoldProductBindingModel
    {
        [Required(ErrorMessage = BussinessLogicConstants.RequiredNumber)]
        public int Number { get; set; }

        [Required(ErrorMessage = BussinessLogicConstants.RequiredOrderType)]
        public string OrderType { get; set; }

        [Required(ErrorMessage = BussinessLogicConstants.RequiredStartDateMessage)]
        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = BussinessLogicConstants.RequiredEndDateMessage)]
        [DateGreaterThan(otherPropertyName = "StartTime", ErrorMessage = BussinessLogicConstants.EndDateMustBeAfteStartDateMessage)]
        [DataType(DataType.Date)]
        public DateTime EndTime { get; set; }
    }
}
