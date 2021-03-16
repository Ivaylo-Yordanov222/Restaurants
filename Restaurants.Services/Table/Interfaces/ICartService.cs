using Restaurants.Common.Table.BindingModels;
using Restaurants.Common.Table.ViewModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Services.Table.Interfaces
{
    public interface ICartService
    {
        Task<ProductViewModel> CheckProduct(string addedProduct);

        Task<decimal> CalculateTotalOrderPriceFromDb(List<ProductViewModel> products);

        Task<string> GetUserIdAsync(ClaimsPrincipal user);

        Task<string> GetUserNameAsync(string userId);

        Task<OrderBindingModel> CompleteOrder(OrderBindingModel order);

        Task<int> GetUserOrdersCountAsync(string userId);
    }
}
