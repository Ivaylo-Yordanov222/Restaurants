using Restaurants.Common.Admin.BindingModels;
using Restaurants.Common.Admin.ViewModels;
using Restaurants.Common.Table.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurants.Services.Admin.Interfaces
{
    public interface IAdminOrdersService
    {
        ICollection<OrderConciseViewModel> GetLastNOrders(int number);

        Task<ICollection<ProductViewModel>> ViewOrderAsync(int orderId);

        Task<ICollection<OrderConciseViewModel>> GetTableOrdersDateToDateAsync(OrderSearchBindingModel model);
    }
}
