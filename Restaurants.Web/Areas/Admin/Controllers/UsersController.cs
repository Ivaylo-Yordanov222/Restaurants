using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Common.Admin.BindingModels;
using Restaurants.Models;
using Restaurants.Services.Admin.Interfaces;
using System.Threading.Tasks;

namespace Restaurants.Web.Areas.Admin.Controllers
{
    public class UsersController : AdminBaseController
    {
        private readonly IAdminUsersService usersService;
        public UsersController(IAdminUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await this.usersService.GetUsersAsync(this.User);
            return this.View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(UsersBindingModel model)
        {
            if(!this.ModelState.IsValid)
            {
                return View();
            }
            var result = await this.usersService.CreateUserAsync(model);
            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }
            return this.RedirectToAction("Index", "Users");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await this.usersService.DeleteUserAsync(id);
            if(result == null)
            {
                return this.BadRequest();
            }
            return this.RedirectToAction("Index", "Users");
        }

    }
}