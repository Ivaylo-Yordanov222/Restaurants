namespace Restaurants.Common.Table.ViewModels
{
    public class PreferencesViewModel
    {
        public int Discount { get; set; } 

        public decimal DiscountMultiplier { get { return (100 - (decimal)this.Discount) / 100; } }

        public int MilisecondsToTakeDiscount { get; set; }

        public  int DisplayItemsPerRow { get; set; }

        public int MaxNumberOfOrdersPerTable { get; set; }

        public int MaxNumberOfItemsInBag { get; set; }
    }
}
