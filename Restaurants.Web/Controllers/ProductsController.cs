using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Common.Constants;
using Restaurants.Common.Table.ViewModels;
using Restaurants.Services.Table.Interfaces;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Restaurants.Web.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductService productService;
        private readonly IPreferencesService preferencesService;

        public ProductsController(IProductService productService, IPreferencesService preferencesService)
        {
            this.productService = productService;
            this.preferencesService = preferencesService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(string slug)
        {
            var model = await productService.DetailsAsync(slug);
            if (model == null)
            {
                return this.NotFound();
            }

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return this.RedirectToAction("Index", "Home");
            }
            if (!Regex.IsMatch(searchTerm,BussinessLogicConstants.ProductAndDescRegexString))
            {
                //logging someone try something stupid
                return this.BadRequest();
            }
            var preferences = await this.preferencesService.GetAppPreferencesAsync();
            IEnumerable<ProductViewModel> model = new List<ProductViewModel>();
            
           
            model = await this.productService.SearchProductByNameAsync(searchTerm);

            this.ViewBag.SearchTerm = searchTerm;
            this.ViewData["ItemsPerRow"] = preferences.DisplayItemsPerRow;
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SearchWithAjax(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return this.RedirectToAction("Index", "Home");
            }
            if (!Regex.IsMatch(searchTerm, BussinessLogicConstants.ProductAndDescRegexString))
            {
                //logging someone try something stupid
                return this.BadRequest();
            }

            var products = await this.productService.SearchWithAjaxAsync(searchTerm);
            if (products == null)
            {
                return NotFound();
            }
            return this.PartialView("_SearchAjaxPartial", products);
        }
    }
}

