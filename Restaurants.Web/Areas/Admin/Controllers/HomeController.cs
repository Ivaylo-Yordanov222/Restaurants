using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Restaurants.Common.Admin.BindingModels;
using Restaurants.Common.Admin.ViewModels;
using Restaurants.Common.Constants;
using Restaurants.Common.Resources;
using Restaurants.Services.Admin.Interfaces;

namespace Restaurants.Web.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        private readonly IAdminUsersService usersService;
        private readonly IAdminOrdersService ordersService;
        private readonly IAdminProductsService productsService;
        private readonly IStringLocalizer<ValidationResources> localizer;

        public HomeController(IAdminUsersService usersService, IAdminOrdersService ordersService, 
            IAdminProductsService productsService, IStringLocalizer<ValidationResources> localizer)
        {
            this.usersService = usersService;
            this.ordersService = ordersService;
            this.productsService = productsService;
            this.localizer = localizer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> ViewOrder(string orderId)
        {
            var model = await this.ordersService.ViewOrderAsync(int.Parse(orderId));
            this.ViewData["orderId"] = orderId;
            return this.PartialView("_ViewOrderPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> SwitchSearchCriteria(string searchCriteria)
        {
            switch (searchCriteria)
            {
                case "search_table_from_date_to_date":
                    var users = await this.usersService.GetUsersListAsync();
                    var usersSelectList = users.Select(c => new SelectListItem { Text = c.UserName, Value = c.Id }).OrderBy(u => u.Text);
                    this.ViewData["users"] = usersSelectList;
                    return this.PartialView("_FormForTableDateToDatePartial", new OrderSearchBindingModel());
                case "most_sold_product":
                    return this.PartialView("_FormForMostSoldProductPartial");
                case "most_sold_table":
                    return this.PartialView("_FormForMostSoldTablePartial");
                case "last_orders":
                    return this.PartialView("_FormForLastOrdersPartial");
                default:
                    return this.NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrdersFromDateToDate(OrderSearchBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var users = await this.usersService.GetUsersListAsync();
                var usersSelectList = users.Select(c => new SelectListItem { Text = c.UserName, Value = c.Id }).OrderBy(u => u.Text);
                this.ViewData["users"] = usersSelectList;
                return this.PartialView("_FormForTableDateToDatePartial", model);
            }
            var result = await this.ordersService.GetTableOrdersDateToDateAsync(model);
            string userName = string.Empty;
            if (result.Count == 0)
            {
                userName = await this.usersService.GetUserAsync(model.UserId);
            }
            else
            {
                userName = result.FirstOrDefault().UserName;
            }
            string foundOrdersForUserFormat = this.localizer[BussinessLogicConstants.FoundOrdersForUserMessage];
            this.ViewData["searchType"] = string.Format(foundOrdersForUserFormat, userName, model.StartTime.ToString("dd/MM/yyyy"),
               model.EndTime.ToString("dd/MM/yyyy"));
            return this.PartialView("_SearchOrdersPartial", result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MostSoldTables(MostSoldTableBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.PartialView("_FormForMostSoldTablePartial", model);
            }
            var result = await this.usersService.GetMostSoldUsersAsync(model);

            var mostSoldTablesFormat = this.localizer[BussinessLogicConstants.MostSoldForTablesMessage];
            this.ViewData["searchType"] = string.Format(mostSoldTablesFormat, model.StartTime.ToString("dd/MM/yyyy"), model.EndTime.ToString("dd/MM/yyyy"));
            return this.PartialView("_MostSoldTablePartial", result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MostSoldProducts(MostSoldProductBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.PartialView("_FormForMostSoldProductPartial", model);
            }
            var result = await this.productsService.GetMostSoldProductsAsync(model);
            var orderedResult = new List<ProductSoldsViewModel>();
            char[] a = model.OrderType.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            string normalizedOrderType = new string(a);
            if (normalizedOrderType == "Quantity")
            {
                orderedResult = result.OrderByDescending(p => p.TotalQuantity).Take(model.Number).ToList();
            }
            else if (normalizedOrderType == "Price")
            {
                orderedResult = result.OrderByDescending(p => p.TotalPrice).Take(model.Number).ToList();
            }
            string mostSoldProductsFormat = this.localizer[BussinessLogicConstants.MostSoldProductsMessage];
            this.ViewData["searchType"] = string.Format(mostSoldProductsFormat, model.StartTime.ToString("dd/MM/yyyy"),
                model.EndTime.ToString("dd/MM/yyyy"), this.localizer[normalizedOrderType]);
            return this.PartialView("_MostSoldProductPartial", orderedResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchLastOrders(NumberBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.PartialView("_FormForLastOrdersPartial", model);
            }

            var result = this.ordersService.GetLastNOrders(model.Number);
            string lastOrdersFormat = this.localizer[BussinessLogicConstants.LastOrdersMessage];
            this.ViewData["searchType"] = string.Format(lastOrdersFormat, result.Count);
            return this.PartialView("_SearchOrdersPartial", result);
        }
    }
}