using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Common.Constants;

namespace Restaurants.Web.Areas.Cooker.Controllers
{
    [Area(BussinessLogicConstants.CookerArea)]
    [Authorize(Roles = BussinessLogicConstants.CookerRole +", " + BussinessLogicConstants.AdminRole)]
    public abstract class CookerBaseController : Controller
    {
    }
}