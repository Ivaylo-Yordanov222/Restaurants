using Restaurants.Common.Admin.BindingModels;
using Restaurants.Common.Admin.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurants.Services.Admin.Interfaces
{
    public interface IAdminCategoryService
    {
        Task<IEnumerable<CategoryConciseViewModel>> GetCategoriesAsync();
        Task<CategoryConciseViewModel> DeleteAsync(string id);

        Task<CategoryConciseViewModel> AddAsync(CategoryBindingModel model);

        Task<CategoryBindingModel> PrepareCategoryForEdit(string id);

        Task<CategoryBindingModel> UpdateAsync(CategoryBindingModel model);

        Task<CategoryDetailsViewModel> DetailsAsync(int categoryId);
    }
}
