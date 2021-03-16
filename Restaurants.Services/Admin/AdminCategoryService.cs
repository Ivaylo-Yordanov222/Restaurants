using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Restaurants.Common.Admin.BindingModels;
using Restaurants.Common.Admin.ViewModels;
using Restaurants.Common.Constants;
using Restaurants.Data;
using Restaurants.Models;
using Restaurants.Services.Admin.Interfaces;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Restaurants.Services.Admin
{
    public class AdminCategoryService : BaseService, IAdminCategoryService
    {
        private readonly IHostingEnvironment hostingEnvironment;
        public AdminCategoryService(RestaurantsContext context, IMapper mapper, IHostingEnvironment hostingEnvironment)
            : base(context, mapper)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public async Task<IEnumerable<CategoryConciseViewModel>> GetCategoriesAsync()
        {
            var categories = await this.DbContext.Categories.ToListAsync();
            var modelCategories = this.Mapper.Map<IEnumerable<CategoryConciseViewModel>>(categories);
            return modelCategories;
        }

        public async Task<CategoryConciseViewModel> DeleteAsync(string id)
        {
            int? number = ExtractDigit(id);
            if (number == null)
            {
                return null;
            }
            var categoryToRemove = await this.DbContext.Categories.FirstOrDefaultAsync(c => c.Id == number);
            var category = this.Mapper.Map<CategoryConciseViewModel>(categoryToRemove);
            if (categoryToRemove == null)
            {
                return null;
            }
            string categoryName = categoryToRemove.Name;
            this.DbContext.Categories.Remove(categoryToRemove);
            this.DbContext.SaveChanges();
            //Delete directory folder
            string categoryPath = Path.Combine(this.hostingEnvironment.WebRootPath, BussinessLogicConstants.ProductsImageFolderPathWithoutSlash, categoryName);
            string categoryTumbPath = Path.Combine(this.hostingEnvironment.WebRootPath, BussinessLogicConstants.ProductsTumbnailsFolderPathWithoutSlash, categoryName);
            if (Directory.Exists(categoryPath))
            {
                Directory.Delete(categoryPath, true);
                if (Directory.Exists(categoryTumbPath))
                {
                    Directory.Delete(categoryTumbPath, true);
                }
            }
            return category;
        }

        public async Task<CategoryConciseViewModel> AddAsync(CategoryBindingModel model)
        {
            var categoryExists = await this.DbContext.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == model.Name.ToLower());
            var result = new CategoryConciseViewModel();
            if (categoryExists != null)
            {
                result = this.Mapper.Map<CategoryConciseViewModel>(categoryExists);
                result.Status = "danger";
                return result;
            }
            var category = this.Mapper.Map<Category>(model);
            await this.DbContext.Categories.AddAsync(category);
            this.DbContext.SaveChanges();
            result.Name = category.Name;
            result.Id = category.Id;
            result.Status = "success";
            //Create category folder
            string categoryPath = Path.Combine(this.hostingEnvironment.WebRootPath, BussinessLogicConstants.ProductsImageFolderPathWithoutSlash, category.Name);
            string categoryTumbPath = Path.Combine(this.hostingEnvironment.WebRootPath, BussinessLogicConstants.ProductsTumbnailsFolderPathWithoutSlash, category.Name);
            if (!Directory.Exists(categoryPath))
            {
                Directory.CreateDirectory(categoryPath);
                if(!Directory.Exists(categoryTumbPath))
                {
                    Directory.CreateDirectory(categoryTumbPath);
                }
            }
            return result;
        }

        public async Task<CategoryBindingModel> PrepareCategoryForEdit(string id)
        {
            int? categoryId = ExtractDigit(id);
            if (categoryId == null)
            {
                return null;
            }
            var category = await this.DbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category == null)
            {
                return null;
            }
            var model = this.Mapper.Map<CategoryBindingModel>(category);
            return model;
        }

        public async Task<CategoryBindingModel> UpdateAsync(CategoryBindingModel model)
        {
            int? categoryId = ExtractDigit(model.Id.ToString());
            if (categoryId == null)
            {
                return null;
            }

            var category = await this.DbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category == null)
            {
                return null;
            }

            if (category.Name.ToLower() != model.Name.ToLower())
            {
                //Update category in db
                var previousCategoryName = category.Name;
                category.Name = model.Name;
                this.DbContext.Categories.Update(category);
                this.DbContext.SaveChanges();

                //Update category's products image path and tumbail path
                var categoryProducts = this.DbContext.Products.Where(p => p.CategoryId == category.Id).ToList();

                foreach (var product in categoryProducts)
                {
                    string fileExtension = Path.GetExtension(product.ImageUrl);
                    product.ImageUrl = Path.Combine("\\" + BussinessLogicConstants.ProductsImageFolderPathWithoutSlash, category.Name, product.Name + fileExtension);
                    product.ImageTumbUrl = Path.Combine("\\" + BussinessLogicConstants.ProductsTumbnailsFolderPathWithoutSlash, category.Name, product.Name + fileExtension);
                    this.DbContext.Products.Update(product);
                }
                this.DbContext.SaveChanges();

                //Update directory folders
                string oldCategoryPath = Path.Combine(this.hostingEnvironment.WebRootPath, BussinessLogicConstants.ProductsImageFolderPathWithoutSlash, previousCategoryName);
                string newDirectoryPath = Path.Combine(this.hostingEnvironment.WebRootPath, BussinessLogicConstants.ProductsImageFolderPathWithoutSlash, model.Name);
                string oldCategoryTumbPath = Path.Combine(this.hostingEnvironment.WebRootPath, BussinessLogicConstants.ProductsTumbnailsFolderPathWithoutSlash, previousCategoryName);
                string newDirectoryTumbPath = Path.Combine(this.hostingEnvironment.WebRootPath, BussinessLogicConstants.ProductsTumbnailsFolderPathWithoutSlash, model.Name);
                if (Directory.Exists(oldCategoryPath))
                {
                    if (!Directory.Exists(newDirectoryPath))
                    {
                        Directory.Move(oldCategoryPath, newDirectoryPath);
                    }
                }
                if (Directory.Exists(oldCategoryTumbPath))
                {
                    if (!Directory.Exists(newDirectoryTumbPath))
                    {
                        Directory.Move(oldCategoryTumbPath, newDirectoryTumbPath);
                    }
                }
            }
            return model;
        }

        public async Task<CategoryDetailsViewModel> DetailsAsync(int categoryId)
        {
            //to do change path to tumbs
            var model = await this.DbContext.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == categoryId);
            var result = this.Mapper.Map<CategoryDetailsViewModel>(model);
            return result;
        }

        private int? ExtractDigit(string s)
        {
            string numbers = new String(s.Where(Char.IsDigit).ToArray());
            if (numbers.Length == 0)
            {
                return null;
            }
            int num = int.Parse(numbers);
            return num;
        }

    }
}
