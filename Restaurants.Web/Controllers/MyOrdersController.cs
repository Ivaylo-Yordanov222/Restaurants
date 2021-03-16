using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Common.Table.ViewModels;
using Restaurants.Services.Table.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurants.Web.Controllers
{
    [Authorize]
    public class MyOrdersController : Controller
    {
        private readonly IMyOrdersService ordersServices;
        private readonly IPreferencesService preferencesService;
        public MyOrdersController(IMyOrdersService ordersServices, IPreferencesService preferencesService)
        {
            this.ordersServices = ordersServices;
            this.preferencesService = preferencesService;
        }

        public async Task<IActionResult> Index()
        {
            var preferences = await this.preferencesService.GetAppPreferencesAsync();

            string userId = await this.ordersServices.CheckUserIdAsync(this.User);
            IEnumerable<OrderViewModel> model = await this.ordersServices.GetOrdersInProcessAsync(userId);
            decimal totalOrdersPrice = 0.0m;
            if (model == null)
            {
                model = new List<OrderViewModel>();
            }
            foreach (var order in model)
            {
                if (order.PromotionPrice != 0.0m)
                {
                    totalOrdersPrice += order.PromotionPrice;
                }
                else
                {
                    totalOrdersPrice += order.Price;
                }
            }
            this.ViewBag.TotalOrdersPrice = totalOrdersPrice;

            this.ViewBag.Discount = preferences.Discount;
            this.ViewData["multiplier"] = preferences.DiscountMultiplier;
            this.ViewBag.MilisecondsToTakeDiscount = preferences.MilisecondsToTakeDiscount;
            return View(model);
        }
    }
}