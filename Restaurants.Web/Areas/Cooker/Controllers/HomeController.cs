using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Services.Cooker.Interfaces;
using Restaurants.Services.Table.Interfaces;

namespace Restaurants.Web.Areas.Cooker.Controllers
{
    public class HomeController : CookerBaseController
    {
        private readonly ICookerOrdersService cookerOrdersService;
        private readonly IMyOrdersService myOrdersService;
        private readonly IPreferencesService preferencesService;

        public HomeController(ICookerOrdersService cookerOrdersService, IMyOrdersService myOrdersService, IPreferencesService preferencesService)
        {
            this.cookerOrdersService = cookerOrdersService;
            this.myOrdersService = myOrdersService;
            this.preferencesService = preferencesService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var preferences = await this.preferencesService.GetAppPreferencesAsync();
            ////////////////////////////////////////////////////////////
            var tables = await this.cookerOrdersService.GetCurrentOrdersAsync();
            foreach(var table in tables)
            {
                table.Orders = await this.myOrdersService.GetOrdersInProcessAsync(table.TableId);
            }
            this.ViewBag.Discount = preferences.Discount;
            this.ViewBag.DiscountMultiplier = preferences.DiscountMultiplier;
            this.ViewBag.MilisecondsToTakeDiscount = preferences.MilisecondsToTakeDiscount;
            return View(tables.OrderBy(t => t.TableRole));
        }
    }
}