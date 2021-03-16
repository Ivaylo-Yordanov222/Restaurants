using Restaurants.Common.Admin.BindingModels;
using Restaurants.Common.Table.ViewModels;
using System.Threading.Tasks;

namespace Restaurants.Services.Table.Interfaces
{
    public interface IPreferencesService
    {
        Task<PreferencesBindingModel> GetPreferencesAsync();

        Task<PreferencesBindingModel> SetPreferencesAsync(PreferencesBindingModel model);

        Task<PreferencesViewModel> GetAppPreferencesAsync();
    }
}
