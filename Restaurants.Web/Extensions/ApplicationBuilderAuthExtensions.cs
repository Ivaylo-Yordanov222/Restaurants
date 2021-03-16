using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Restaurants.Common.Constants;
using Restaurants.Data;
using Restaurants.Models;
using Restaurants.Web.Models.Dto;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurants.Web.Extensions
{
    public static class ApplicationBuilderAuthExtensions
    {

        private static readonly IdentityRole[] roles =
        {
            new IdentityRole(BussinessLogicConstants.AdminRole),
            new IdentityRole(BussinessLogicConstants.CookerRole)
        };
        public static  IApplicationBuilder SeedData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<RestaurantsContext>();

                SeedDatabaseRolesAndUsers(scope);
                SeedDatabaseCategories(dbContext);
                SeedDatabaseProducts(dbContext);
                SeedDatabasePreferences(dbContext);
            }
            return app;
        }

        private static void SeedDatabaseProducts(RestaurantsContext dbContext)
        {
            if (!dbContext.Products.Any())
            {
                //string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, WebConstants.ProductsImageFolderPath);

                var productsFile = File.ReadAllText(BussinessLogicConstants.ProductsPath);
                var deserializedProducts = JsonConvert.DeserializeObject<ProductDto[]>(productsFile);
               
                var products = deserializedProducts.Select(p => new Product
                {
                    Name = p.Name,
                    Slug = p.Slug,
                    Description = p.Description,
                    ImageUrl = Path.Combine(BussinessLogicConstants.ProductsImageFolderPathWithSlash, p.ImageUrl),
                    ImageTumbUrl = Path.Combine(BussinessLogicConstants.ProductsTumbnailsFolderPathWithSlash, p.ImageTumbUrl),
                    CategoryId = p.CategoryId,
                    Price = p.Price
                }).ToArray();

                dbContext.Products.AddRange(products);
                dbContext.SaveChanges();
            }
        }

        private static void SeedDatabaseCategories(RestaurantsContext dbContext)
        {
            if (!dbContext.Categories.Any())
            {
                var categoriesFile = File.ReadAllText(BussinessLogicConstants.CategoryPath);
                var deserializedCategories = JsonConvert.DeserializeObject<Category[]>(categoriesFile);

                dbContext.Categories.AddRange(deserializedCategories);
                dbContext.SaveChanges();
            }
        } 
        private static void SeedDatabasePreferences(RestaurantsContext dbContext)
        {
            if (!dbContext.Preferences.Any())
            {
                var preferences = new Preference
                {
                    Discount = 10,
                    MilisecondsToTakeDiscount = 3600000,
                    DisplayItemsPerRow = 4,
                    MaxNumberOfOrdersPerTable = 4,
                    MaxNumberOfItemsInBag = 20
                };
                dbContext.Preferences.Add(preferences);
                dbContext.SaveChanges();
            }
        }

        private static  void SeedDatabaseRolesAndUsers(IServiceScope scope)
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            Task
                .Run(async () =>
                {
                    foreach (var role in roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role.Name))
                        {
                            await roleManager.CreateAsync(role);
                        }
                    }

                    var admin = await userManager.FindByNameAsync(BussinessLogicConstants.AdminDefaultUsername);
                    if (admin == null)
                    {
                        admin = new User()
                        {
                            UserName = BussinessLogicConstants.AdminDefaultUsername,
                            Email = BussinessLogicConstants.AdminDefaultEmail,
                            EmailConfirmed = true
                        };
                        await userManager.CreateAsync(admin, BussinessLogicConstants.AdminDefaultPassword);
                        await userManager.AddToRoleAsync(admin, roles[0].Name);
                    }

                    var cooker = await userManager.FindByNameAsync(BussinessLogicConstants.CookerDefaultUsername);
                    if (cooker == null)
                    {
                        cooker = new User()
                        {
                            UserName = BussinessLogicConstants.CookerDefaultUsername,
                            Email = BussinessLogicConstants.CookerDefaultEmail,
                            EmailConfirmed = true
                        };
                        await userManager.CreateAsync(cooker, BussinessLogicConstants.CookerDefaultPassword);
                        await userManager.AddToRoleAsync(cooker, roles[1].Name);
                    }

                    var normalUser = await userManager.FindByNameAsync("Rumi");
                    if (normalUser == null)
                    {
                        normalUser = new User
                        {
                            UserName = "Rumi",
                            Email = "rumi@mail.bg",
                            EmailConfirmed = true
                        };
                        await userManager.CreateAsync(normalUser, "rumi123");
                    }
                }).Wait();
        }
    }
}
