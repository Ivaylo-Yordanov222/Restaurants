using Restaurants.Common.Table.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Restaurants.Services.Table.Interfaces
{
    public interface IMyOrdersService
    {
        Task<IEnumerable<OrderViewModel>> GetOrdersInProcessAsync(string userId);

        Task<string> CheckUserIdAsync(ClaimsPrincipal user);
    }
}
