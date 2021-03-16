using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurants.Common.Constants;

namespace Restaurants.Web.Areas.Identity.Pages.Account
{
    [Authorize(Roles = BussinessLogicConstants.AdminRole)]
    public class BasePage: PageModel
    {
    }
}
