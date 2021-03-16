using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Restaurants.Common.Admin.BindingModels;
using Restaurants.Common.Admin.ViewModels;
using Restaurants.Common.Constants;
using Restaurants.Common.Enums;
using Restaurants.Common.Utilities;
using Restaurants.Data;
using Restaurants.Models;
using Restaurants.Services.Admin.Interfaces;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurants.Services.Admin
{
    public class AdminProductsService : BaseService, IAdminProductsService
    {
        private readonly IHostingEnvironment hostingEnvironment;
        public AdminProductsService(RestaurantsContext context, IMapper mapper, IHostingEnvironment hostingEnvironment)
            : base(context, mapper)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> AddProductAsync(ProductBindingModel model)
        {
            string[] filePath = UploadedFile(model);
            if (filePath[0] == null || filePath[1] == null)
            {
                return null;
            }

            var product = this.Mapper.Map<Product>(model);
            product.ImageUrl = filePath[0];
            product.ImageTumbUrl = filePath[1];
            product.Discount = (1 - (model.PromotionalPrice / model.Price)) * 100;
            product.Id = 0;

            await this.DbContext.Products.AddAsync(product);
            this.DbContext.SaveChanges();

            return "Success";
        }

        public async Task<int?> DeleteAsync(int id)
        {
            var productToDelete = await this.DbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (productToDelete == null)
                return null;
            int categoryId = productToDelete.CategoryId;
            string categoryName = this.DbContext.Categories.FirstOrDefault(c => c.Id == categoryId).Name;
            this.DbContext.Products.Remove(productToDelete);
            this.DbContext.SaveChanges();

            //Deleting the file from the file system
            string extension = Path.GetExtension(productToDelete.ImageUrl);
            string fullFilePath = Path.Combine(this.hostingEnvironment.WebRootPath, BussinessLogicConstants.ProductsImageFolderPathWithoutSlash, categoryName, productToDelete.Name + extension);
            string fullFileTumbPath = Path.Combine(this.hostingEnvironment.WebRootPath, BussinessLogicConstants.ProductsTumbnailsFolderPathWithoutSlash, categoryName, productToDelete.Name + extension);
            if (File.Exists(fullFilePath))
            {
                File.Delete(fullFilePath);
            }
            if (File.Exists(fullFileTumbPath))
            {
                File.Delete(fullFileTumbPath);
            }
            return categoryId;
        }

        public async Task<ProductBindingModel> PrepareProductModelForEditingAsync(int id)
        {
            var product = await this.DbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return null;
            }
            var result = new ProductBindingModel()
            {
                Name = product.Name,
                Slug = product.Slug,
                Description = product.Description,
                CategoryId = product.CategoryId,
                OldPictureUrl = product.ImageUrl,
                OldTubmPictureUrl = product.ImageTumbUrl,
                PromotionalPrice = product.PromotionalPrice,
                Price = product.Price
            };
            return result;
        }

        public async Task<string> UpdateAsync(ProductBindingModel model)
        {
            string[] filePath = UploadedFile(model);
            if (filePath[0] == null || filePath[1] == null)
            {
                return null;
            }
            string result = string.Empty;
            var product = this.Mapper.Map<Product>(model);

            product.ImageUrl = filePath[0];
            product.ImageTumbUrl = filePath[1];
            product.Discount = (1 - (model.PromotionalPrice / model.Price)) * 100;
            this.DbContext.Products.Update(product);
            await this.DbContext.SaveChangesAsync();
            result = "Success";
            return result;
        }

        private string[] UploadedFile(ProductBindingModel model)
        {
            string resultPath = null;
            string resultTumbPath = null;
            string[] paths = new string[2];
            CategoryConciseViewModel category = null;
            if (model.ImageUrl != null)
            {
                if (model.Categories != null)
                {
                    category = model.Categories.FirstOrDefault(c => c.Id == model.CategoryId);
                }
                else
                {
                    var dbCategory = this.DbContext.Categories.FirstOrDefault(c => c.Id == model.CategoryId);
                    if (dbCategory == null)
                        return null;
                    category = this.Mapper.Map<CategoryConciseViewModel>(dbCategory);
                }
                if (category == null)
                {
                    return null;
                }
                string categoryName = category.Name;
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, BussinessLogicConstants.ProductsImageFolderPathWithoutSlash, categoryName);
                string uploadsTumbFolder = Path.Combine(hostingEnvironment.WebRootPath, BussinessLogicConstants.ProductsTumbnailsFolderPathWithoutSlash, categoryName);
                string extensionName = Path.GetExtension(model.ImageUrl.FileName);
                string fileName = model.Name + extensionName;
                string filePath = Path.Combine(uploadsFolder, fileName);
                string fileTubmPath = Path.Combine(uploadsTumbFolder, fileName);
                //delete old image in folder and tumb image
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                if (File.Exists(fileTubmPath))
                {
                    File.Delete(fileTubmPath);
                }
                if (model.OldPictureUrl != null)
                {
                    DeleteOldFile(model.OldPictureUrl);
                    DeleteOldFile(model.OldTubmPictureUrl);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImageUrl.CopyTo(fileStream);
                }
                resultPath = Path.Combine("\\", BussinessLogicConstants.ProductsImageFolderPathWithoutSlash, categoryName, fileName);
                using (Image image = Image.FromFile(filePath))
                {
                    Image resultImage = ResizeTumbnails.ResizeImage(image, new Size(300, 300), preserveAspectRatio: true);
                    if (!Directory.Exists(uploadsTumbFolder))
                    {
                        Directory.CreateDirectory(uploadsTumbFolder);
                    }
                    resultImage.Save(fileTubmPath);
                }
                resultTumbPath = Path.Combine("\\", BussinessLogicConstants.ProductsTumbnailsFolderPathWithoutSlash, categoryName, fileName);
                paths[0] = resultPath;
                paths[1] = resultTumbPath;
            }
            return paths;
        }

        private void DeleteOldFile(string imageUrl)
        {
            imageUrl = imageUrl.Trim('\\');
            var oldFilePath = Path.Combine(this.hostingEnvironment.WebRootPath, imageUrl);
            File.Delete(oldFilePath);
        }

        public async Task<ICollection<ProductSoldsViewModel>> GetMostSoldProductsAsync(MostSoldProductBindingModel model)
        {
            var orders = await this.DbContext.Orders.
                Include(o => o.ProductsInOrder)
                .ThenInclude(po => po.Product)
                .ThenInclude(p => p.Category)
                .Where(o => o.StartTime > model.StartTime && o.EndTime < model.EndTime && o.Status == Status.Paid)
                .ToListAsync();
            
            var listOfProducts = new Dictionary<int, ProductSoldsViewModel>();
            var productsResultList = new List<ProductSoldsViewModel>();

            foreach (var order in orders)
            {
                foreach (var productInOrder in order.ProductsInOrder)
                {
                    if (!listOfProducts.ContainsKey(productInOrder.Product.Id))
                    {
                        var productToAdd = this.Mapper.Map<ProductSoldsViewModel>(productInOrder.Product);
                        listOfProducts[productInOrder.Product.Id] = productToAdd;
                        productsResultList.Add(productToAdd);
                    }

                    var currentListWithSales = new ProductPricesViewModel()
                    {
                        OrderId = order.Id,
                        Quantity = productInOrder.Quantity,
                        SoldPrice = productInOrder.SoldPrice,
                        OrderDiscount = productInOrder.Discount,
                        ProductDiscount = productInOrder.ProductDiscount
                    };
                    listOfProducts[productInOrder.Product.Id].Solds.Add(currentListWithSales);
                }
            }
            return productsResultList;
        }
    }
}
