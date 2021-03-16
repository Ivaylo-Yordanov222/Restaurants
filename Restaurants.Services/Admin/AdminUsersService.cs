using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurants.Common.Admin.BindingModels;
using Restaurants.Common.Admin.ViewModels;
using Restaurants.Common.Constants;
using Restaurants.Common.Enums;
using Restaurants.Data;
using Restaurants.Models;
using Restaurants.Services.Admin.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Restaurants.Services.Admin
{
    public class AdminUsersService : BaseService, IAdminUsersService
    {
        private readonly UserManager<User> userManager;

        public AdminUsersService(RestaurantsContext context, IMapper mapper, UserManager<User> userManager)
            : base(context, mapper)
        {
            this.userManager = userManager;
        }
        public async Task<string> GetUserAsync(string userId)
        {
            var user = await this.DbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            string userName = await this.userManager.GetUserNameAsync(user);
            return userName;
        }

        public async Task<IEnumerable<UsersViewModel>> GetUsersAsync(ClaimsPrincipal user)
        {
            var currentUser = await this.userManager.GetUserAsync(user);
            var users = await this.DbContext.Users.ToListAsync();

            var dictionaryWithRoles = new Dictionary<string, string>();
            List<User> usersResult = new List<User>();
            foreach (var u in users)
            {
                if (userManager.IsInRoleAsync(u, BussinessLogicConstants.AdminRole).Result)
                {
                    continue;
                }
                if (userManager.GetRolesAsync(u).Result.Contains(BussinessLogicConstants.CookerRole))
                {
                    dictionaryWithRoles.Add(u.Id, BussinessLogicConstants.CookerRole);
                    usersResult.Add(u);
                }
                else
                {
                    dictionaryWithRoles.Add(u.Id, BussinessLogicConstants.TableRole);
                    usersResult.Add(u);
                }
            }
            var model = this.Mapper.Map<IEnumerable<UsersViewModel>>(usersResult);
            foreach (var modelUser in model)
            {
                if (dictionaryWithRoles.ContainsKey(modelUser.Id))
                {
                    modelUser.Role = dictionaryWithRoles[modelUser.Id];
                }
            }
            return model;
        }

        public async Task<IdentityResult> CreateUserAsync(UsersBindingModel model)
        {
            var user = new User
            {
                UserName = model.Username,
                Fullname = model.Fullname,
                Email = model.Email,
                EmailConfirmed = true
            };
            var result = await this.userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (model.Role == 1)
                {
                    await this.userManager.AddToRoleAsync(user, BussinessLogicConstants.CookerRole);
                }
            }

            return result;

        }

        public async Task<string> DeleteUserAsync(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            await this.userManager.DeleteAsync(user);
            return "Success";
        }

        public async Task<IEnumerable<UsersConciseViewModel>> GetUsersListAsync()
        {
            var users = await this.DbContext.Users.ToListAsync();
            var model = Mapper.Map<IEnumerable<UsersConciseViewModel>>(users);
            return model;
        }

        public async Task<ICollection<UsersSoldViewModel>> GetMostSoldUsersAsync(MostSoldTableBindingModel model)
        {
            var users = await this.DbContext.Users
                .Include(u => u.Orders).ToListAsync();

            var result = new List<UsersSoldViewModel>();
            foreach (var user in users)
            {
                string roleType = string.Empty;
                decimal userTotalPrice = 0.0m;
                var orders = user.Orders.Where(o => o.StartTime > model.StartTime && o.EndTime < model.EndTime && o.Status == Status.Paid).ToList();
                foreach (var order in orders)
                {
                    if (order.PromotionPrice != 0.0m)
                    {
                        userTotalPrice += order.PromotionPrice;
                    }
                    else
                    {
                        userTotalPrice += order.Price;
                    }
                }

                if (userManager.IsInRoleAsync(user, BussinessLogicConstants.AdminRole).Result)
                {
                    roleType = BussinessLogicConstants.AdminRole;
                }
                else if (userManager.IsInRoleAsync(user, BussinessLogicConstants.CookerRole).Result)
                {
                    roleType = BussinessLogicConstants.CookerRole;
                }
                else
                {
                    roleType = BussinessLogicConstants.TableRole;
                }
                var resultUser = new UsersSoldViewModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = roleType,
                    OrderCount = orders.Count,
                    TotalSoldPrice = userTotalPrice
                };
                result.Add(resultUser);
            }
            return result.OrderByDescending(u => u.TotalSoldPrice).ThenBy(u => u.UserName).ToList();
        }

    }
}
