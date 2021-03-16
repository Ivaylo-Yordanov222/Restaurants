using Restaurants.Common.Cooker.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurants.Services.Cooker.Interfaces
{
    public interface ICookerOrdersService
    {
        Task<IEnumerable<TablesAndOrdersViewModel>> GetCurrentOrdersAsync();

        Task<int[]> Agree(int orderId);

        string GetUserId(int orderId);
    }
}
