using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Restaurants.Common.Admin.BindingModels;
using Restaurants.Common.Constants;
using Restaurants.Common.Resources;
using Restaurants.Services.Admin.Interfaces;

namespace Restaurants.Web.Areas.Admin.Controllers
{
    public class CategoryController : AdminBaseController
    {
        private readonly IAdminCategoryService categoryService;
        private readonly IStringLocalizer<ValidationResources> localizer;

        public CategoryController(IAdminCategoryService categoryService, IStringLocalizer<ValidationResources> localizer)
        {
            this.categoryService = categoryService;
            this.localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var models = await categoryService.GetCategoriesAsync();
            return View(models.OrderByDescending(m => m.Id));
        }
        [HttpGet]
        public IActionResult Add()
        {
            return this.PartialView("_CategoryAddPartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CategoryBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return PartialView("_CategoryAddPartial", model);
            }
            var result = await this.categoryService.AddAsync(model);
            if (result.Status == "success")
            {
                var messageFormat = this.localizer[BussinessLogicConstants.AddCategoryMessage];
                result.Message = string.Format(messageFormat, result.Name);
                return this.PartialView("_CategoryRowPartial", result);
            }
            else if(result.Status == "danger")
            {
                var messageFormat = this.localizer[BussinessLogicConstants.AddCategoryExistsMessage];
                result.Message = string.Format(messageFormat, result.Name);
                return Json(result);
            }
            else
            {
                return this.BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await this.categoryService.PrepareCategoryForEdit(id);
            if (model == null)
            {
                return this.NotFound();
            }

            return PartialView("_CategoryEditingPartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CategoryBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.PartialView("_CategoryEditingPartial", model);
            }

            var result = await this.categoryService.UpdateAsync(model);
            if (result == null)
            {
                return this.NotFound();
            }
            return Json(result);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await this.categoryService.DeleteAsync(id);
            if (result == null)
            {
                return this.NotFound();
            }
            string formatMessage = this.localizer[BussinessLogicConstants.DeleteCategoryMessage];
            return this.Content(string.Format(formatMessage, result.Name));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await this.categoryService.DetailsAsync(id);

            if (model == null)
            {
                return this.NotFound();
            }

            model.Categories = await this.categoryService.GetCategoriesAsync();
            model.Products = model.Products.OrderByDescending(p => p.Id).ToArray();

            return this.View(model);
        }
    }
}