using System.Threading.Tasks;

namespace Restaurants.Web.Utilities.Interfaces
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}
