using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Restaurants.Common.Constants;
using Restaurants.Common.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurants.Web.Utilities
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        private readonly IStringLocalizer<ValidationResources> localizer;
        public CustomIdentityErrorDescriber(IStringLocalizer<ValidationResources> localizer)
        {
            this.localizer = localizer;
        }
        public override IdentityError DefaultError() { return new IdentityError { Code = nameof(DefaultError), Description = localizer[BussinessLogicConstants.DefaultError] }; }
        public override IdentityError ConcurrencyFailure() { return new IdentityError { Code = nameof(ConcurrencyFailure), Description = localizer[BussinessLogicConstants.ConcurrencyFailure] }; }
        public override IdentityError PasswordMismatch() { return new IdentityError { Code = nameof(PasswordMismatch), Description = localizer[BussinessLogicConstants.PasswordMismatch] }; }
        public override IdentityError InvalidToken() { return new IdentityError { Code = nameof(InvalidToken), Description = localizer[BussinessLogicConstants.InvalidToken] }; }
        public override IdentityError LoginAlreadyAssociated() { return new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = localizer[BussinessLogicConstants.LoginAlreadyAssociated] }; }
        public override IdentityError InvalidUserName(string userName)
        {
            string errorMessage = String.Format(localizer[BussinessLogicConstants.InvalidUserName], userName);
            return new IdentityError { Code = nameof(InvalidUserName), Description = errorMessage };
        }
        public override IdentityError InvalidEmail(string email)
        {
            string errorMessage = String.Format(localizer[BussinessLogicConstants.InvalidEmail], email);
            return new IdentityError { Code = nameof(InvalidEmail), Description = errorMessage };
        }
        public override IdentityError DuplicateUserName(string userName)
        {
            string errorMessage = String.Format(localizer[BussinessLogicConstants.DuplicateUserName], userName);
            return new IdentityError { Code = nameof(DuplicateUserName), Description = errorMessage };
        }
        public override IdentityError DuplicateEmail(string email)
        {
            string errorMessage = String.Format(localizer[BussinessLogicConstants.DuplicateEmail], email);
            return new IdentityError { Code = nameof(DuplicateEmail), Description = errorMessage };
        }
        public override IdentityError InvalidRoleName(string role)
        {
            string errorMessage = String.Format(localizer[BussinessLogicConstants.InvalidRoleName], role);
            return new IdentityError { Code = nameof(InvalidRoleName), Description = errorMessage };
        }
        public override IdentityError DuplicateRoleName(string role)
        {
            string errorMessage = String.Format(localizer[BussinessLogicConstants.DuplicateRoleName], role);
            return new IdentityError { Code = nameof(DuplicateRoleName), Description = errorMessage };
        }
        public override IdentityError UserAlreadyHasPassword()
        {
            return new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = localizer[BussinessLogicConstants.UserAlreadyHasPassword] };
        }
        public override IdentityError UserLockoutNotEnabled() { return new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = localizer[BussinessLogicConstants.UserLockoutNotEnabled] }; }
        public override IdentityError UserAlreadyInRole(string role)
        {
            string errorMessage = String.Format(localizer[BussinessLogicConstants.UserAlreadyInRole], role);
            return new IdentityError { Code = nameof(UserAlreadyInRole), Description = errorMessage };
        }
        public override IdentityError UserNotInRole(string role)
        {
            string errorMessage = String.Format(localizer[BussinessLogicConstants.UserNotInRole], role);
            return new IdentityError { Code = nameof(UserNotInRole), Description = errorMessage };
        }
        public override IdentityError PasswordTooShort(int length) 
        {
            string errorMessage = String.Format(localizer[BussinessLogicConstants.PasswordTooShort], length);
            return new IdentityError { Code = nameof(PasswordTooShort), Description = errorMessage }; 
        }
        public override IdentityError PasswordRequiresNonAlphanumeric() { return new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = localizer[BussinessLogicConstants.PasswordRequiresNonAlphanumeric] }; }
        public override IdentityError PasswordRequiresDigit() { return new IdentityError { Code = nameof(PasswordRequiresDigit), Description = localizer[BussinessLogicConstants.PasswordRequiresDigit] }; }
        public override IdentityError PasswordRequiresLower() { return new IdentityError { Code = nameof(PasswordRequiresLower), Description = localizer[BussinessLogicConstants.PasswordRequiresLower] }; }
        public override IdentityError PasswordRequiresUpper() { return new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "Passwords must have at least one uppercase ('A'-'Z')." }; }
    }
}
