using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurants.Common.Enums;
using Restaurants.Common.Table.BindingModels;
using Restaurants.Common.Table.ViewModels;
using Restaurants.Data;
using Restaurants.Models;
using Restaurants.Services.Table.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Restaurants.Services.Table
{
    public class CartService : BaseService, ICartService
    {
        private readonly UserManager<User> userManager;

        public CartService(RestaurantsContext context, IMapper mapper, UserManager<User> userManager)
        : base(context, mapper)
        {
            this.userManager = userManager;
        }

        public async Task<decimal> CalculateTotalOrderPriceFromDb(List<ProductViewModel> products)
        {
            var totalOrderPrice = 0.0m;
            foreach (var product in products)
            {
                var dbProduct = await this.DbContext.Products.FirstOrDefaultAsync(p => p.Name.ToLower() == product.Name.ToLower());

                if (dbProduct.PromotionalPrice != 0m)
                {
                    totalOrderPrice += dbProduct.PromotionalPrice * product.Quantity;
                }
                else
                {
                    totalOrderPrice += dbProduct.Price * product.Quantity;
                }
            }

            return totalOrderPrice;
        }

        public async Task<ProductViewModel> CheckProduct(string addedProduct)
        {
            var product = await this.DbContext.Products
                .FirstOrDefaultAsync(p => p.Name.ToLower() == addedProduct.ToLower());
            if (product == null)
            {
                return null;
            }
            var result = this.Mapper.Map<ProductViewModel>(product);
            result.CategoryName = GetCategoryNameById(result.CategoryId);
            return result;

        }

        public async Task<string> GetUserIdAsync(ClaimsPrincipal user)
        {
            User dbUser = await this.userManager.GetUserAsync(user);
            string dbUserId = dbUser.Id;
            if (dbUserId == null)
            {
                return null;
            }
            return dbUserId;
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            User dbUser = await this.userManager.FindByIdAsync(userId);
            return dbUser.UserName;
        }

        public async Task<OrderBindingModel> CompleteOrder(OrderBindingModel order)
        {
            var dbOrder = new Order
            {
                UserId = order.UserId,
                Status = Status.Sent,
                Price = order.Price,
                StartTime = DateTime.UtcNow
            };

            await this.DbContext.Orders.AddAsync(dbOrder);
            this.DbContext.SaveChanges();

            foreach (var product in order.Products)
            {
                var OrdersAndProducts = new ProductsOrders
                {
                    OrderId = dbOrder.Id,
                    ProductId = product.Id,
                    Quantity = product.Quantity
                };
                this.DbContext.ProductsOrders.Add(OrdersAndProducts);
            }
            await this.DbContext.SaveChangesAsync();
            order.OrderId = dbOrder.Id;
            order.StartTime = dbOrder.StartTime;
            order.DateAndTime = dbOrder.StartTime.ToString("yyyy-MM-ddTHH:mm:ss");
            order.Status = dbOrder.Status;
            return order;
        }

        public async Task<int> GetUserOrdersCountAsync(string userId)
        {
            int result = await this.DbContext.Orders
                .Where(o => o.UserId == userId && o.Status != Status.Paid)
                .CountAsync();
            return result;
        }

        private string GetCategoryNameById(int id)
        {
            return this.DbContext.Categories.FirstOrDefaultAsync(c => c.Id == id).Result.Name;
        }
    }
}
