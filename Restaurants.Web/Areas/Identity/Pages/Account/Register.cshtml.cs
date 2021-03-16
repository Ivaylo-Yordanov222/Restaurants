using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Restaurants.Common.Constants;
using Restaurants.Models;

namespace Restaurants.Web.Areas.Identity.Pages.Account
{
    public class RegisterModel : BasePage
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = BussinessLogicConstants.RequiredField)]
            [EmailAddress]
            [Display(Name = BussinessLogicConstants.EmailDisplayMessage)]
            public string Email { get; set; }

            [Required(ErrorMessage = BussinessLogicConstants.RequiredField)]
            [Display(Name = BussinessLogicConstants.UserNameDisplayMessage)]
            [RegularExpression(BussinessLogicConstants.UserNameRegexString, ErrorMessage = BussinessLogicConstants.UserNameValidationRegex)]
            public string Username { get; set; }

            //[Required(ErrorMessage = BussinessLogicConstants.RequiredField)]
            [Display(Name = BussinessLogicConstants.FullNameDisplayMessage)]
            [RegularExpression(BussinessLogicConstants.UserNameRegexString, ErrorMessage = BussinessLogicConstants.FullNameValidationRegex)]
            public string Fullname { get; set; }

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

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new User { UserName = Input.Username, Email = Input.Email, Fullname = Input.Fullname, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    return this.RedirectToAction("Index", "Users", new { area = BussinessLogicConstants.AdminArea });
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = user.Id, code = code },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    //{
                    //    return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    //}
                    //else
                    //{
                    //    await _signInManager.SignInAsync(user, isPersistent: false);
                    //    return LocalRedirect(returnUrl);
                    //}
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
