using Microsoft.AspNetCore.Identity;
using Restaurants.Common.Admin.BindingModels;
using Restaurants.Common.Admin.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Restaurants.Services.Admin.Interfaces
{
    public interface IAdminUsersService
    {
        Task<IEnumerable<UsersViewModel>> GetUsersAsync(ClaimsPrincipal user);

        Task<string> GetUserAsync(string userId);

        Task<IdentityResult> CreateUserAsync(UsersBindingModel model);

        Task<string> DeleteUserAsync(string userId);

        Task<IEnumerable<UsersConciseViewModel>> GetUsersListAsync();

        Task<ICollection<UsersSoldViewModel>> GetMostSoldUsersAsync(MostSoldTableBindingModel model);
    }
}
