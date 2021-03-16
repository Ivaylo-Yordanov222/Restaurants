using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Restaurants.Common.Table.ViewModels;
using Restaurants.Services.Admin.Interfaces;
using Restaurants.Services.Table.Interfaces;
using Restaurants.Web.Models;

namespace Restaurants.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService productService;
        private readonly IAdminCategoryService categoryService;
        private readonly IPreferencesService preferencesService;
        private readonly IStringLocalizer<HomeController> localizer;

        public HomeController(ILogger<HomeController> logger, IProductService productService, IAdminCategoryService categoryService,
            IPreferencesService preferencesService, IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            this.productService = productService;
            this.categoryService = categoryService;
            this.preferencesService = preferencesService;
            this.localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string categoryName, int? page)
        {
            string title = this.localizer["Index"];
            var preferences = await this.preferencesService.GetAppPreferencesAsync();
            int count = preferences.DisplayItemsPerRow * 4;
            if (!page.HasValue)
            {
                page = 1;
            }
            page = await this.productService.CheckPageValueAsync(page.Value, count);
            if (page == -1)
            {
                return this.NotFound();
            }
            IEnumerable<ProductViewModel> products = new List<ProductViewModel>();
            int numberOfPages = 0;

            var categories = await this.categoryService.GetCategoriesAsync();
            if (categoryName == null)
            {
                numberOfPages = await this.productService.GetNumberOfPagesAsync(count);
                products = productService.GetProductsPagination(page.Value,count);
            }
            else
            {
                products = await productService.GetProductsFromCategoryAsync(categoryName, page.Value, count);
                if(products == null)
                {
                    return this.BadRequest();
                }
                numberOfPages = await this.productService.GetNumberOfPagesByCategoryAsync(categoryName, count);
            }
            var model = new ProductsPaginationViewModel()
            {
                Page = page.Value,
                CategoryName = categoryName,
                NumberOfPages = numberOfPages,
                Categories = categories,
                Products = products
            };
            this.ViewData["ItemsPerRow"] = preferences.DisplayItemsPerRow;
            return View(model);

        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions() 
                { 
                    Expires = DateTimeOffset.UtcNow.AddYears(1) 
                }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
