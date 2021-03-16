using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Restaurants.Common.Admin.BindingModels;
using Restaurants.Common.Constants;
using Restaurants.Common.Enums;
using Restaurants.Common.Resources;
using Restaurants.Common.Utilities;
using Restaurants.Services.Table.Interfaces;
using Restaurants.Web.Extensions;
using System.Threading.Tasks;

namespace Restaurants.Web.Areas.Admin.Controllers
{
    public class PreferencesController : AdminBaseController
    {
        private readonly IPreferencesService preferencesService;
        private IStringLocalizer<ValidationResources> localizer;
        public PreferencesController(IPreferencesService preferencesService, IStringLocalizer<ValidationResources> localizer)
        {
            this.localizer = localizer;
            this.preferencesService = preferencesService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await this.preferencesService.GetPreferencesAsync();
            if (model == null)
            {
                return this.BadRequest();
            }
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(PreferencesBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            var result = await this.preferencesService.SetPreferencesAsync(model);
            if(result == null)
            {
                return this.BadRequest();
            }
            this.TempData.Put(BussinessLogicConstants.MessageKey, new MessageModel()
            {
                Type = MessageType.Success,
                Message = this.localizer[BussinessLogicConstants.PreferencesTempDataMessage]
            });
            return this.RedirectToAction("Index", "Home");
        }
    }
}
