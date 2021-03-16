using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurants.Common.Cooker.ViewModels;
using Restaurants.Common.Enums;
using Restaurants.Data;
using Restaurants.Models;
using Restaurants.Services.Cooker.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Restaurants.Common.Constants;
using System;
using Restaurants.Services.Table.Interfaces;

namespace Restaurants.Services.Cooker
{
    public class CookerOrdersService : BaseService, ICookerOrdersService
    {
        private readonly UserManager<User> userManager;
        private readonly IPreferencesService preferencesService;
        public CookerOrdersService(RestaurantsContext context, IMapper mapper, UserManager<User> userManager, IPreferencesService preferencesService)
            : base(context, mapper)
        {
            this.userManager = userManager;
            this.preferencesService = preferencesService;
        }

        public async Task<IEnumerable<TablesAndOrdersViewModel>> GetCurrentOrdersAsync()
        {
            List<User> allUsers = new List<User>();
            await GetSpecialUsers(allUsers);

            await GetNormalUsers(allUsers);

            var tables = new List<TablesAndOrdersViewModel>();
            foreach (var u in allUsers)
            {
                string userRole = "table";
                if (userManager.IsInRoleAsync(u, BussinessLogicConstants.AdminRole).Result)
                {
                    userRole = "Admin";
                }
                else if (userManager.IsInRoleAsync(u, BussinessLogicConstants.CookerRole).Result)
                {
                    userRole = "Cooker";
                }

                var table = new TablesAndOrdersViewModel()
                {
                    TableName = u.UserName,
                    TableId = u.Id,
                    TableRole = userRole
                };
                tables.Add(table);
            }

            return tables;
        }
        public async Task<int[]> Agree(int orderId)
        {
            var preferences = await this.preferencesService.GetAppPreferencesAsync();
            Order order = null;
            try
            {
                order = await this.DbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                {
                    return new int[] { -1, 0 };
                }
                switch (order.Status)
                {
                    case Status.Sent:
                        order.Status = Status.InProgress;
                        break;
                    case Status.InProgress:
                        order.Status = Status.Delivered;
                        order.DeliverTime = DateTime.UtcNow;
                        var productsOrders = await this.DbContext.ProductsOrders
                                .Include(po => po.Product)
                                .Where(po => po.OrderId == orderId).ToListAsync();
                        if (!IsDeliveredInTime(order, preferences.MilisecondsToTakeDiscount))
                        {
                            order.PromotionPrice = order.Price * preferences.DiscountMultiplier;
                            order.Discount = preferences.Discount;

                            foreach (var productOrder in productsOrders)
                            {
                                productOrder.Discount = preferences.Discount;
                                if (productOrder.Product.PromotionalPrice != 0m)
                                {
                                    productOrder.SoldPrice = productOrder.Product.PromotionalPrice * preferences.DiscountMultiplier;
                                    productOrder.ProductDiscount = productOrder.Product.Discount;
                                }
                                else
                                {
                                    productOrder.SoldPrice = productOrder.Product.Price * preferences.DiscountMultiplier;
                                }
                            }
                        }
                        else
                        {
                            foreach (var productOrder in productsOrders)
                            {
                                if (productOrder.Product.PromotionalPrice != 0m)
                                {
                                    productOrder.SoldPrice = productOrder.Product.PromotionalPrice;
                                    productOrder.ProductDiscount = productOrder.Product.Discount;
                                }
                                else
                                {
                                    productOrder.SoldPrice = productOrder.Product.Price;
                                }
                            }
                        }
                        break;
                    case Status.Delivered:
                        order.Status = Status.Paid;
                        order.EndTime = DateTime.UtcNow;
                        break;
                    default:
                        break;
                }
                this.DbContext.SaveChanges();
            }
            catch
            {
                return new int[] { -1, 0 };
            }
            return new int[] { orderId, (int)order.Status };
        }
        public string GetUserId(int orderId)
        {
            var order = this.DbContext.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                return null;
            }
            return order.UserId;
        }

        private async Task GetSpecialUsers(List<User> allUsers)
        {
            List<User> specialUsers = new List<User>();

            var usersWithActiveOrders = await this.DbContext.Users
                .Include(u => u.Orders)
                .Where(u => u.Orders.Count() > 0 && u.Orders.Where(o => o.Status != Status.Paid).Count() > 0)
                .ToListAsync();

            foreach (var u in usersWithActiveOrders)
            {
                if (userManager.IsInRoleAsync(u, BussinessLogicConstants.AdminRole).Result || userManager.IsInRoleAsync(u, BussinessLogicConstants.CookerRole).Result)
                {
                    specialUsers.Add(u);
                }
            }
            allUsers.AddRange(specialUsers);
        }

        private async Task GetNormalUsers(List<User> allUsers)
        {
            var normalUsers = await this.DbContext.Users.ToListAsync();
            foreach (var u in normalUsers)
            {
                if (!userManager.IsInRoleAsync(u, BussinessLogicConstants.AdminRole).Result && !userManager.IsInRoleAsync(u, BussinessLogicConstants.CookerRole).Result)
                {
                    allUsers.Add(u);
                }
            }
        }

        private bool IsDeliveredInTime(Order order, int minutesToTakeTheDiscount)
        {
            var deliveredTime = order.DeliverTime - new DateTime(1970, 1, 1);
            var orderedTime = order.StartTime - new DateTime(1970, 1, 1);
            var difference = deliveredTime - orderedTime;
            if (difference.Value.TotalMilliseconds > minutesToTakeTheDiscount)
            {
                return false;
            }

            return true;
        }

    }
}
