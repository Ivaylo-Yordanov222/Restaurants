using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurants.Common.Admin.ViewModels;
using Restaurants.Common.Table.ViewModels;
using Restaurants.Data;
using Restaurants.Models;
using Restaurants.Services.Table.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurants.Services.Table
{
    public class ProductService : BaseService, IProductService
    {
        public ProductService(RestaurantsContext context, IMapper mapper)
            : base(context, mapper)
        { }

        public async Task<ProductDetailsViewModel> DetailsAsync(string slug)
        {
            var product = await this.DbContext.Products.FirstOrDefaultAsync(p => p.Slug == slug);
            if (product == null)
            {
                return null;
            }
            var model = this.Mapper.Map<ProductDetailsViewModel>(product);
            model.CategoryName = this.GetCategoryNameById(model.CategoryId);

            return model;
        }

        public async Task<int> CheckPageValueAsync(int page, int count)
        {
            var pageCount = await GetNumberOfPagesAsync(count);
            if (page <= 0 || page > pageCount)
            {
                return -1;
            }

            return page;
        }

        public IEnumerable<ProductViewModel> GetProductsPagination(int page, int count)
        {
            var products = this.DbContext.Products
                .Include(p => p.Category)
                .Skip((page - 1) * count)
                .Take(count)
                .OrderBy(p => p.Name)
                .ToList();
            var result = this.Mapper.Map<IEnumerable<ProductViewModel>>(products);

            return result;
        }

        public async Task<IEnumerable<ProductViewModel>> GetProductsFromCategoryAsync(string categoryName, int page, int count)
        {
            var category = await GetCategory(categoryName);
            if (category == null)
            {
                return null;
            }
            var products = this.DbContext.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == category.Id)
                .Skip((page - 1) * count)
                .Take(count)
                .OrderBy(p => p.Name)
                .ToList();

            var results = this.Mapper.Map<IEnumerable<ProductViewModel>>(products);

            return results;
        }

        public async Task<int> GetNumberOfPagesAsync(int count)
        {
            var numberOfProducts = await this.DbContext.Products.CountAsync();
            int numberOfPages = 0;
            if (numberOfProducts % count == 0)
            {
                numberOfPages = (numberOfProducts / count);
            }
            else
            {
                numberOfPages = ( numberOfProducts/ count) + 1;
            }
            return numberOfPages;
        }

        public async Task<int> GetNumberOfPagesByCategoryAsync(string categoryName, int count)
        {
            var category = await GetCategory(categoryName);

            var numberOfProducts = await this.DbContext.Products.Where(p => p.CategoryId == category.Id).CountAsync();
            int numberOfPages = 0;
            if (numberOfProducts % count == 0)
            {
                numberOfPages = (numberOfProducts / count);
            }
            else
            {
                numberOfPages = (numberOfProducts / count) + 1;
            }
            return numberOfPages;
        }

        public async Task<IEnumerable<ProductViewModel>> SearchProductByNameAsync(string searchTerm)
        {
            var products = await this.DbContext.Products
                .Where(p => p.Name.ToLower().Contains(searchTerm.ToLower()))
                .Include(p => p.Category)
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Slug = p.Slug,
                    ImageUrl = p.ImageUrl,
                    ImageTumbUrl = p.ImageTumbUrl,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    PromotionalPrice = p.PromotionalPrice,
                    Discount = p.Discount,
                    Price = p.Price
                }).ToListAsync();

            return products;
        }

        public async Task<IEnumerable<ProductConciseViewModel>> SearchWithAjaxAsync(string searchTerm)
        {
            var products = await this.DbContext.Products
              .Where(p => p.Name.ToLower().Contains(searchTerm.ToLower())).ToListAsync();

            var results = this.Mapper.Map<IEnumerable<ProductConciseViewModel>>(products);

            return results;
        }

        //to do capsolation
        private async Task<Category> GetCategory(string categoryName)
        {
            return await this.DbContext.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower());
        }

        //to do capsolation
        private string GetCategoryNameById(int id)
        {
            return this.DbContext.Categories.FirstOrDefaultAsync(c => c.Id == id).Result.Name;
        }
    }
}
