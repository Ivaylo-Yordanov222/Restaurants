using Restaurants.Common.Admin.BindingModels;
using Restaurants.Common.Admin.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurants.Services.Admin.Interfaces
{
    public interface IAdminProductsService
    {
        Task<string> AddProductAsync(ProductBindingModel model);

        Task<int?> DeleteAsync(int id);

        Task<ProductBindingModel> PrepareProductModelForEditingAsync(int id);

        Task<string> UpdateAsync(ProductBindingModel model);

        Task<ICollection<ProductSoldsViewModel>> GetMostSoldProductsAsync(MostSoldProductBindingModel model);
    }
}
