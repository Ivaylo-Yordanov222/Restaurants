using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurants.Common.Admin.BindingModels;
using Restaurants.Common.Table.ViewModels;
using Restaurants.Data;
using Restaurants.Models;
using Restaurants.Services.Table.Interfaces;
using System.Threading.Tasks;

namespace Restaurants.Services.Table
{
    public class PreferencesService: BaseService, IPreferencesService
    {
        private readonly UserManager<User> userManager;

        public PreferencesService(RestaurantsContext context, IMapper mapper, UserManager<User> userManager)
       : base(context, mapper)
        {
            this.userManager = userManager;
        }

        public async Task<PreferencesViewModel> GetAppPreferencesAsync()
        {
            var preferences =  await this.DbContext.Preferences.FirstOrDefaultAsync();
            if (preferences == null)
            {
                return null;
            }
            var result = this.Mapper.Map<PreferencesViewModel>(preferences);
            return result;
        }

        public async Task<PreferencesBindingModel> GetPreferencesAsync()
        {
            var preferences = await this.DbContext.Preferences.FirstOrDefaultAsync();
            if (preferences == null)
            {
                return null;
            }
            var result = this.Mapper.Map<PreferencesBindingModel>(preferences);
            return result;
        }

        public async Task<PreferencesBindingModel> SetPreferencesAsync(PreferencesBindingModel model)
        {
            var preferences = await this.DbContext.Preferences.FirstOrDefaultAsync();
            if(preferences == null)
            {
                return null;
            }
            this.Mapper.Map(model, preferences);
            this.DbContext.Preferences.Update(preferences);
            this.DbContext.SaveChanges();
            
            var result = model;
            return result;
        }
    }
}
