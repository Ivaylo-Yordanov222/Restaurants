using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Restaurants.Common.Constants;
using Restaurants.Common.Cooker.ViewModels;
using Restaurants.Common.Enums;
using Restaurants.Common.Resources;
using Restaurants.Common.Table.BindingModels;
using Restaurants.Common.Table.ViewModels;
using Restaurants.Common.Utilities;
using Restaurants.Services.Table.Interfaces;
using Restaurants.Web.Extensions;
using Restaurants.Web.Hubs;
using Restaurants.Web.Utilities.Interfaces;

namespace Restaurants.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService cartService;
        private readonly IHubContext<OrdersHub> hubContext;
        private readonly IPreferencesService preferencesService;
        private readonly IViewRenderService viewRender;
        private readonly IStringLocalizer<ValidationResources> localizer;

        public CartController(ICartService cartService, IHubContext<OrdersHub> hubContext, IPreferencesService preferencesService,
            IViewRenderService viewRender,
            IStringLocalizer<ValidationResources> localizer)
        {
            this.cartService = cartService;
            this.hubContext = hubContext;
            this.preferencesService = preferencesService;
            this.viewRender = viewRender;
            this.localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var preferences = await this.GetPreferences();
            var products = this.HttpContext.Session.Get<List<ProductViewModel>>(BussinessLogicConstants.SessionKeyForCartProductAdding);
            if (products == null)
            {
                products = new List<ProductViewModel>();
            }

            decimal totalPrice = CalculateTotalPrice(products);

            this.ViewBag.TotalPrice = totalPrice;
            this.ViewBag.MaxItemsInBag = preferences.MaxNumberOfItemsInBag;

            return View(products);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(string addedProduct)
        {
            var preferences = await this.GetPreferences();
            var product = await this.cartService.CheckProduct(addedProduct);
            if (product == null)
            {
                return this.NotFound();
            }
            var products = this.HttpContext.Session.Get<List<ProductViewModel>>(BussinessLogicConstants.SessionKeyForCartProductAdding);
            if (products == null)
            {
                products = new List<ProductViewModel>();
            }
            else if (products.Select(p => p.Quantity).Sum() + product.Quantity > preferences.MaxNumberOfItemsInBag)
            {
                var messageFormat = this.localizer[BussinessLogicConstants.MaxNumberOfItemsInBagMessage];
                this.TempData.Put(BussinessLogicConstants.MessageKey, new MessageModel()
                {
                    Type = MessageType.Delete,
                    Message = string.Format(messageFormat, preferences.MaxNumberOfItemsInBag)
                });
                return this.RedirectToAction("Index", "Cart");
            }
            if (products.Any(p => p.Name.ToLower() == product.Name.ToLower()))
            {
                product = products.FirstOrDefault(p => p.Name.ToLower() == product.Name.ToLower());
                product.Quantity++;
                if (product.Quantity > 50)
                {
                    product.Quantity = 50;
                }
            }
            else
            {
                products.Add(product);
            }
            this.HttpContext.Session.Put(BussinessLogicConstants.SessionKeyForCartProductAdding, products);
            return this.RedirectToAction("Index", "Cart");
        }

        [HttpGet]
        public IActionResult Delete(string productToRemove)
        {
            var products = this.HttpContext.Session.Get<List<ProductViewModel>>(BussinessLogicConstants.SessionKeyForCartProductAdding);
            if (products == null)
            {
                return this.BadRequest();
            }
            var product = products.FirstOrDefault(p => p.Name.ToLower() == productToRemove.ToLower());
            if (product == null)
            {
                return this.BadRequest();
            }

            products.Remove(product);
            this.HttpContext.Session.Put(BussinessLogicConstants.SessionKeyForCartProductAdding, products);
            return this.RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProductInSession(string productName, int quantity)
        {
            var preferences = await GetPreferences();
            if (quantity < 1 || quantity > 50)
            {
                return this.BadRequest();
            }

            var products = this.HttpContext.Session.Get<List<ProductViewModel>>(BussinessLogicConstants.SessionKeyForCartProductAdding);
            var productToUpdate = products.FirstOrDefault(p => p.Name.ToLower() == productName.ToLower());
            if (productToUpdate == null)
            {
                return this.BadRequest();
            }
            if ((products.Select(p => p.Quantity).Sum() - productToUpdate.Quantity) + quantity > preferences.MaxNumberOfItemsInBag)
            {
                var messageFormat = this.localizer[BussinessLogicConstants.MaxNumberOfItemsInBagMessage];
                this.TempData.Put(BussinessLogicConstants.MessageKey, new MessageModel()
                {
                    Type = MessageType.Delete,
                    Message = string.Format(messageFormat, preferences.MaxNumberOfItemsInBag)
                });
                return this.BadRequest();
            }
            productToUpdate.Quantity = quantity;

            decimal totalPrice = CalculateTotalPrice(products);

            this.HttpContext.Session.Put(BussinessLogicConstants.SessionKeyForCartProductAdding, products);
            int count = this.UpdateQuantityInCart();
            return this.Content(totalPrice + this.localizer[BussinessLogicConstants.BgPriceIndicator] + ";" + count);
        }

        [HttpPost]
        [AllowAnonymous]
        public int UpdateQuantityInCart()
        {
            var products = this.HttpContext.Session.Get<List<ProductViewModel>>(BussinessLogicConstants.SessionKeyForCartProductAdding);
            if (products == null)
            {
                return 0;
            }
            int quantity = products.Sum(p => p.Quantity);
            return quantity;
        }

        [HttpGet]
        public IActionResult Check()
        {
            var products = this.HttpContext.Session.Get<List<ProductViewModel>>(BussinessLogicConstants.SessionKeyForCartProductAdding);
            if (products == null || products.Count() == 0)
            {
                return this.RedirectToAction("Index", "Cart");
            }

            decimal totalPrice = CalculateTotalPrice(products);

            this.ViewBag.TotalPrice = totalPrice;
            return this.View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout()
        {
            ///////////////////
            var preferences = await this.GetPreferences();
            /////////////////
            var products = this.HttpContext.Session.Get<List<ProductViewModel>>(BussinessLogicConstants.SessionKeyForCartProductAdding);
            if (products == null)
            {
                return this.RedirectToAction("Index", "Cart");
            }

            decimal totalPrice = await this.cartService.CalculateTotalOrderPriceFromDb(products);

            string userId = await this.cartService.GetUserIdAsync(this.User);

            if (userId == null)
            {
                return this.RedirectToAction("Index", "Cart");
            }
            //////////////////////////////////////////////////////
            int activeOrdersCount = await this.cartService.GetUserOrdersCountAsync(userId);
            if (activeOrdersCount >= preferences.MaxNumberOfOrdersPerTable)
            {
                var messageFormat = this.localizer[BussinessLogicConstants.MaxNumberOfOrdersPerUserMessage];
                this.TempData.Put(BussinessLogicConstants.MessageKey, new MessageModel()
                {
                    Type = MessageType.Delete,
                    Message = string.Format(messageFormat, preferences.MaxNumberOfOrdersPerTable)
                });
                return this.RedirectToAction("Index", "MyOrders");
            }
            //////////////////////////////////////////////////////
            string userName = await this.cartService.GetUserNameAsync(userId);
            var orderModel = new OrderBindingModel
            {
                UserId = userId,
                Username = userName,
                Price = totalPrice,
                Products = products
            };

            var completedOrder = await this.cartService.CompleteOrder(orderModel);

            var orderViewModel = new OrderViewModel()
            {
                OrderId = completedOrder.OrderId,
                Username = completedOrder.Username,
                StartTime = completedOrder.StartTime,
                Price = completedOrder.Price,
                Status = completedOrder.Status,
                Products = completedOrder.Products
            };
            string razorOrderPartial = await this.viewRender.RenderToStringAsync(@"\Areas\Cooker\Views\Shared\_OrdersCookerPartial.cshtml", orderViewModel);
            string uName = orderViewModel.Username;
            var result = new SignalROrder()
            {
                Username = uName,
                OrderId = orderViewModel.OrderId,
                OrderHtml = razorOrderPartial
            };

            this.HttpContext.Session.Put(BussinessLogicConstants.SessionKeyForCartProductAdding, new List<ProductViewModel>());
            var jsonOrder = JsonConvert.SerializeObject(result);

            await this.hubContext.Clients.Group("Cookers").SendAsync("ShowOrder", jsonOrder);

            return this.RedirectToAction("Index", "MyOrders");
        }

        private static decimal CalculateTotalPrice(List<ProductViewModel> products)
        {
            decimal totalPrice = 0.0m;
            foreach (var product in products)
            {
                if (product.PromotionalPrice != 0m)
                {
                    totalPrice += product.PromotionalPrice * product.Quantity;
                }
                else
                {
                    totalPrice += product.Price * product.Quantity;
                }
            }

            return totalPrice;
        }

        private async Task<PreferencesViewModel> GetPreferences()
        {
            var preferences = await this.preferencesService.GetAppPreferencesAsync();
            return preferences;
        }

    }
}