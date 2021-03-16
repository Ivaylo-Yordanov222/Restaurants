using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Common.Constants;

namespace Restaurants.Web.Areas.Admin.Controllers
{
    [Area(BussinessLogicConstants.AdminArea)]
    [Authorize(Roles = BussinessLogicConstants.AdminRole)]
    public abstract class AdminBaseController : Controller
    {
        
    }
}