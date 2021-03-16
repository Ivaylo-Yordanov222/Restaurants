using Microsoft.EntityFrameworkCore;
using Restaurants.Common.Admin.BindingModels;
using Restaurants.Common.Admin.ViewModels;
using Restaurants.Common.Enums;
using Restaurants.Common.Table.ViewModels;
using Restaurants.Data;
using Restaurants.Services.Admin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Restaurants.Services.Admin
{
    public class AdminOrdersService : BaseService, IAdminOrdersService
    {
        public AdminOrdersService(RestaurantsContext context, IMapper mapper)
            : base(context, mapper)
        { }

        public ICollection<OrderConciseViewModel> GetLastNOrders(int number)
        {
            if (number == 0)
            {
                number = 3;
            }
            var orders = this.DbContext.Orders.Where(o => o.Status == Status.Paid)
                .Include(o => o.User)
                .OrderBy(o => o.EndTime)
                .ToList();
            var resultOrders = orders.Skip(Math.Max(0, orders.Count() - number)).ToList();
            var model = this.Mapper.Map<ICollection<OrderConciseViewModel>>(resultOrders);
            return model;
        }

        public async Task<ICollection<OrderConciseViewModel>> GetTableOrdersDateToDateAsync(OrderSearchBindingModel model)
        {
            var orders = await this.DbContext.Orders
                .Include(o => o.User)
                .Where(o => o.UserId == model.UserId && o.StartTime > model.StartTime && o.EndTime < model.EndTime && o.Status == Status.Paid)
                .ToListAsync();
            var result = this.Mapper.Map<ICollection<OrderConciseViewModel>>(orders);
            return result;
        }

        public async Task<ICollection<ProductViewModel>> ViewOrderAsync(int orderId)
        {
            var order = await this.DbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
            List<ProductViewModel> products = new List<ProductViewModel>();

            products = await this.DbContext.Orders
           .Where(o => o.Id == orderId)
           .Include(o => o.ProductsInOrder)
           .ThenInclude(po => po.Product)
           .ThenInclude(c => c.Category)
           .Select(om => om.ProductsInOrder.Select(po => new ProductViewModel
           {
               Id = po.ProductId,
               Quantity = po.Quantity,
               Name = po.Product.Name,
               Slug = po.Product.Slug,
               ImageUrl = po.Product.ImageUrl,
               ImageTumbUrl = po.Product.ImageTumbUrl,
               Price = po.SoldPrice,
               PromotionalPrice = po.SoldPrice,
               Discount = po.Product.Discount,
               CategoryId = po.Product.Category.Id,
               CategoryName = po.Product.Category.Name
           }).ToList()
           ).FirstOrDefaultAsync();

            return products;
        }
    }
}
