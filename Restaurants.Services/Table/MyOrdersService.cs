using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using Microsoft.EntityFrameworkCore;
using Restaurants.Common.Enums;
using Restaurants.Common.Table.ViewModels;
using Restaurants.Data;
using Restaurants.Models;
using Restaurants.Services.Table.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Restaurants.Services.Table
{
    public class MyOrdersService : BaseService, IMyOrdersService
    {
        private readonly UserManager<User> userManager;

        public MyOrdersService(RestaurantsContext context, IMapper mapper, UserManager<User> userManager)
        : base(context, mapper)
        {
            this.userManager = userManager;
        }

        public async Task<IEnumerable<OrderViewModel>> GetOrdersInProcessAsync(string userId)
        {
            var orders = await this.DbContext.Orders
                .Where(o => o.UserId == userId && o.Status != Status.Paid)
                .Include(o => o.ProductsInOrder)
                .ThenInclude(po => po.Product)
                .ThenInclude(c => c.Category)
                .Select(om => new OrderViewModel
                {
                    OrderId = om.Id,
                    Price = om.Price,
                    PromotionPrice = om.PromotionPrice,
                    StartTime = om.StartTime,
                    DeliverTime = om.DeliverTime,
                    Status = om.Status,
                    Products = om.ProductsInOrder
                    .Select(po => new ProductViewModel
                    {
                        Id = po.ProductId,
                        Quantity = po.Quantity,
                        Name = po.Product.Name,
                        Slug = po.Product.Slug,
                        ImageUrl = po.Product.ImageUrl,
                        ImageTumbUrl = po.Product.ImageTumbUrl,
                        PromotionalPrice = po.Product.PromotionalPrice,
                        Price = po.Product.Price,
                        Discount = po.Product.Discount,
                        CategoryId = po.Product.Category.Id,
                        CategoryName = po.Product.Category.Name
                    })
                    .ToList()
                }).ToListAsync();

            return orders;
        }

        public async Task<string> CheckUserIdAsync(ClaimsPrincipal user)
        {
            User dbUser = await this.userManager.GetUserAsync(user);
            string dbUserId = dbUser.Id;
            if (dbUserId == null)
            {
                return null;
            }
            return dbUserId;
        }
    }
}
