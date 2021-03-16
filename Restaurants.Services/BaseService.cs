using AutoMapper;
using Restaurants.Data;

namespace Restaurants.Services
{
    public abstract class BaseService
    {
        protected BaseService(RestaurantsContext context, IMapper mapper)
        {
            this.DbContext = context;
            this.Mapper = mapper;
        }

        protected RestaurantsContext DbContext { get; private set; }
        protected IMapper Mapper { get; private set; }
    }
}
