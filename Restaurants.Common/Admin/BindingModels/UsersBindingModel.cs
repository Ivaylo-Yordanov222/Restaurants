using Restaurants.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Restaurants.Common.Admin.BindingModels
{
    public class UsersBindingModel
    {
        [Required(ErrorMessage = BussinessLogicConstants.RequiredField)]
        [EmailAddress]
        [Display(Name = BussinessLogicConstants.EmailDisplayMessage)]
        public string Email { get; set; }

        [Required (ErrorMessage = BussinessLogicConstants.RequiredField)]
        [Display(Name = BussinessLogicConstants.UserNameDisplayMessage)]
        [RegularExpression(BussinessLogicConstants.UserNameRegexString, ErrorMessage = BussinessLogicConstants.UserNameValidationRegex)]
        public string Username { get; set; }

        [Display(Name = BussinessLogicConstants.FullNameDisplayMessage)]
        [RegularExpression(BussinessLogicConstants.FullNameRegexString, ErrorMessage = BussinessLogicConstants.FullNameValidationRegex)]
        public string Fullname { get; set; }

        [Required]
        [Display(Name = BussinessLogicConstants.RoleDisplayMessage)]
        public int Role { get; set; }

        [Required(ErrorMessage = BussinessLogicConstants.RequiredField)]
        [StringLength(100, ErrorMessage = BussinessLogicConstants.PasswordStringLengthValidationMessage, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = BussinessLogicConstants.PasswordDisplayMessage)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = BussinessLogicConstants.ConfirmPasswordDisplayMessage)]
        [Compare("Password", ErrorMessage = BussinessLogicConstants.ConfirmPasswordCompareValidatingMessage)]
        public string ConfirmPassword { get; set; }
    }
}
