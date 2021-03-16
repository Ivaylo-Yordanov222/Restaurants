using Restaurants.Common.Admin.ViewModels;
using Restaurants.Common.Table.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurants.Services.Table.Interfaces
{
    public interface IProductService
    {
        Task<ProductDetailsViewModel> DetailsAsync(string slug);

        IEnumerable<ProductViewModel> GetProductsPagination(int page, int count);

        Task<IEnumerable<ProductViewModel>> GetProductsFromCategoryAsync(string categoryName,int page, int count);

        Task<IEnumerable<ProductViewModel>> SearchProductByNameAsync(string searchTerm);

        Task<IEnumerable<ProductConciseViewModel>> SearchWithAjaxAsync(string searchTerm);

        Task<int> CheckPageValueAsync(int page, int count);

        Task<int> GetNumberOfPagesAsync(int count);

        Task<int> GetNumberOfPagesByCategoryAsync(string categoryName, int count);
    }
}
