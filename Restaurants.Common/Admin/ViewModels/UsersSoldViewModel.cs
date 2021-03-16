namespace Restaurants.Common.Admin.ViewModels
{
    public class UsersSoldViewModel
    { 
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public int OrderCount { get; set; }

        public decimal TotalSoldPrice { get; set; }
    }
}
