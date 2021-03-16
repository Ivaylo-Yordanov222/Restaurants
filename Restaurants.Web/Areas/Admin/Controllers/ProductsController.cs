using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Localization;
using Restaurants.Common.Admin.BindingModels;
using Restaurants.Common.Admin.ViewModels;
using Restaurants.Common.Constants;
using Restaurants.Common.Enums;
using Restaurants.Common.Resources;
using Restaurants.Common.Utilities;
using Restaurants.Services.Admin.Interfaces;
using Restaurants.Web.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurants.Web.Areas.Admin.Controllers
{
    public class ProductsController : AdminBaseController
    {
        private readonly IAdminProductsService productsService;
        private readonly IAdminCategoryService categoryService;
        private readonly IStringLocalizer<ValidationResources> localizer;
        public ProductsController(IAdminProductsService productsService, IAdminCategoryService categoryService, IStringLocalizer<ValidationResources> localizer)
        {
            this.productsService = productsService;
            this.categoryService = categoryService;
            this.localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> Add(int? id)
        {
            var productModel = new ProductBindingModel();

            var selectListModel = await this.PrepareCategoryList(productModel);

            if (id != null)
            {
                var selectElement = selectListModel.FirstOrDefault(el => el.Value == id.ToString());
                if (selectElement != null)
                {
                    selectElement.Selected = true;
                }
                productModel.CategoryId = productModel.Categories.FirstOrDefault(c => c.Id == id).Id;
            }

            this.ViewData["Categories"] = selectListModel;

            return this.View(productModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProductBindingModel model)
        {
            var selectListModel = await PrepareCategoryList(model);
            if (!this.ModelState.IsValid)
            {
                this.ViewData["Categories"] = selectListModel;
                return this.View(model);
            }

            string message = await productsService.AddProductAsync(model);

            if (message.ToLower() == "success")
            {
                var messageFormat = this.localizer[BussinessLogicConstants.AddProductTempDataMessage].ToString();
                this.TempData.Put(BussinessLogicConstants.MessageKey, new MessageModel()
                {
                    Type = MessageType.Success,
                    Message = string.Format(messageFormat, model.Name, model.CategoryId)
                });

                return this.RedirectToAction("Details", "Category", new { id = model.CategoryId });
            }
            else
            {
                return this.BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            int? result = await this.productsService.DeleteAsync(id);
            if (result == null)
            {
                this.NotFound();
            }
            this.TempData.Put("__Message", new MessageModel()
            {
                Type = MessageType.Delete,
                Message = this.localizer[BussinessLogicConstants.DeleteProductTempDataMessage]
            });
            return this.RedirectToAction("Details", "Category", new { id = result });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await this.productsService.PrepareProductModelForEditingAsync(id);
            if (model == null)
            {
                return this.NotFound();
            }
            model.Categories = await this.categoryService.GetCategoriesAsync();
            var selectListModel = await PrepareCategoryList(model);
            this.ViewData["Categories"] = selectListModel;
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }
            string result = await this.productsService.UpdateAsync(model);
            if (result != "Success")
            {
                return this.BadRequest();
            }
            this.TempData.Put(BussinessLogicConstants.MessageKey, new MessageModel()
            {
                Type = MessageType.Success,
                Message = this.localizer[BussinessLogicConstants.EditingProductTempDataMessage]
            });
            return this.RedirectToAction("Details", "Category", new { id = model.CategoryId });
        }

        private async Task<List<SelectListItem>> PrepareCategoryList(ProductBindingModel model)
        {
            model.Categories = await this.categoryService.GetCategoriesAsync();
            var selectListModel = model.Categories.Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() }).ToList();
            return selectListModel;
        }
    }
}