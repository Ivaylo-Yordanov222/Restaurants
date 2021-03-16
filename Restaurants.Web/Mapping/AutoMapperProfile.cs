using AutoMapper;
using Restaurants.Models;
using Restaurants.Common.Admin.BindingModels;
using Restaurants.Common.Admin.ViewModels;
using Restaurants.Common.Table.ViewModels;

namespace Restaurants.Web.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category, CategoryConciseViewModel>();
            CreateMap<Category, CategoryBindingModel>();
            CreateMap<Category, CategoryDetailsViewModel>();
            CreateMap<CategoryConciseViewModel, Category>();
            CreateMap<CategoryBindingModel, Category>();

            CreateMap<Product, ProductConciseViewModel>();
            CreateMap<Product, ProductSoldsViewModel>();
            CreateMap<Product, ProductViewModel>();
            CreateMap<Product, ProductDetailsViewModel>();
            CreateMap<ProductBindingModel, Product>();

            CreateMap<User, UsersViewModel>();
            CreateMap<User, UsersConciseViewModel>();
            CreateMap<UsersBindingModel, User>();

            CreateMap<Order, OrderConciseViewModel>().ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<Preference, PreferencesBindingModel>();
            CreateMap<PreferencesBindingModel, Preference>();
            CreateMap<Preference, PreferencesViewModel>();
        }
    }
}