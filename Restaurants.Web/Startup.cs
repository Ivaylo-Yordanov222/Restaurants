using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Restaurants.Models;
using Restaurants.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Restaurants.Web.Areas.Identity.Services;
using Restaurants.Web.Extensions;
using AutoMapper;
using Restaurants.Web.Mapping;
using Restaurants.Services.Admin.Interfaces;
using Restaurants.Services.Admin;
using Restaurants.Services.Table.Interfaces;
using Restaurants.Services.Table;
using Restaurants.Web.Hubs;
using Restaurants.Services.Cooker.Interfaces;
using Restaurants.Services.Cooker;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Razor;
using Restaurants.Common.Resources;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Common.Utilities;
using Restaurants.Web.Utilities.Interfaces;
using Restaurants.Services.Utilities;
using Restaurants.Web.Utilities;

namespace Restaurants.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            CultureInfo[] supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("bg")
            };

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;

            });

            services.AddDbContext<RestaurantsContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("RestaurantsConnection"),
                    dbOptions => dbOptions.MigrationsAssembly("Restaurants.Data")));

            services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddDefaultUI()
                .AddEntityFrameworkStores<RestaurantsContext>()
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddDefaultTokenProviders();

            //External logins
            services.AddAuthentication()
             .AddFacebook(options =>
             {
                 //Can use user secrets
                 options.AppId = this.Configuration.GetSection("ExternalAuthentication:Facebook:AppId").Value;
                 options.AppSecret = this.Configuration.GetSection("ExternalAuthentication:Facebook:AppSecret").Value;
             })
             .AddGoogle(options =>
             {
                 options.ClientId = this.Configuration.GetSection("ExternalAuthentication:Google:ClientId").Value; ;
                 options.ClientSecret = this.Configuration.GetSection("ExternalAuthentication:Google:ClientSecret").Value;
             });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password = new PasswordOptions()
                {
                    RequiredLength = 4,
                    RequiredUniqueChars = 1,
                    RequireLowercase = true,
                    RequireDigit = false,
                    RequireNonAlphanumeric = false,
                    RequireUppercase = false
                };

                options.SignIn.RequireConfirmedEmail = true;
            });

            //For many servers sessions
            services.AddDistributedMemoryCache();
            services.AddSession();
            //Email Sender
            services.AddSingleton<IEmailSender, SendGridEmailSender>();
            services.Configure<SendGridOptions>(this.Configuration.GetSection("EmailSettings"));

            services.AddAutoMapper(c => c.AddProfile<AutoMapperProfile>(), typeof(Startup));

            // Adding Service Layer
            CreateServicesLayer(services);

            services.AddSignalR();

            services.AddMvc(options => {
                options.ModelBinderProviders.Insert(0, new CustomBinderProvider());
            })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(ValidationResources));
                });
                
            services.AddRazorPages();
        }

        private static void CreateServicesLayer(IServiceCollection services)
        {
            //admin services
            services.AddScoped<IAdminCategoryService, AdminCategoryService>();
            services.AddScoped<IAdminProductsService, AdminProductsService>();
            services.AddScoped<IAdminUsersService, AdminUsersService>();
            services.AddScoped<IAdminOrdersService, AdminOrdersService>();
            //table services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IMyOrdersService, MyOrdersService>();
            //cooker services
            services.AddScoped<ICookerOrdersService, CookerOrdersService>();
            //preferences service
            services.AddScoped<IPreferencesService, PreferencesService>();
            //Rendering view service
            services.AddScoped<IViewRenderService, ViewRenderService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseRequestLocalization();
            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            if (env.IsDevelopment())
            {
                //app.SeedData();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<OrdersHub>("/orders");

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{slug?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
