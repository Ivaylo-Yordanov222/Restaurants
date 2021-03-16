using System;

namespace Restaurants.Common.Admin.ViewModels
{
    public class ProductPricesViewModel
    {
        public int OrderId { get; set; }

        public int Quantity { get; set; }

        public decimal SoldPrice { get; set; }

        public int OrderDiscount { get; set; }

        public decimal ProductDiscount { get; set; }

        public decimal RealPrice
        {
            get
            {
                decimal result = 0m;
                if(ProductDiscount > 0m && OrderDiscount > 0)
                {
                    var priceWithoutOrderDiscount = SoldPrice / ((100 - (decimal)OrderDiscount) / 100);
                    result = priceWithoutOrderDiscount / ((100 - ProductDiscount) / 100);
                    return Convert.ToDecimal(String.Format("{0:###0.00}",result));
                }
                else if(OrderDiscount > 0)
                {
                    result = SoldPrice / ((100 - (decimal)OrderDiscount) / 100);
                    return Convert.ToDecimal(String.Format("{0:###0.00}", result));
                }
                else if(ProductDiscount > 0m)
                {
                    result = SoldPrice / ((100 - ProductDiscount) / 100);
                    return Convert.ToDecimal(String.Format("{0:###0.00}", result));
                }
                else
                {
                    result = SoldPrice;
                    return result;
                }
            }
        }

        public decimal PromotionalPrice
        {
            get
            {
                decimal result = 0m;
                if (ProductDiscount == 0m && OrderDiscount == 0)
                {
                    return Convert.ToDecimal(String.Format("{0:###0.00}", result));
                }
                else if (ProductDiscount > 0m && OrderDiscount == 0)
                {
                    result = RealPrice * ((100 - ProductDiscount) / 100);
                    return Convert.ToDecimal(String.Format("{0:###0.00}", result));
                }
                else if (ProductDiscount == 0m && OrderDiscount != 0)
                {
                    result = RealPrice * ((100 - (decimal)OrderDiscount) / 100);
                    return Convert.ToDecimal(String.Format("{0:###0.00}", result));
                }
                else
                {
                    result = RealPrice * ((100 - ProductDiscount) / 100);
                    return Convert.ToDecimal(String.Format("{0:###0.00}", result));
                }
            }
        }
        public decimal Total
        {
            get
            {
                return this.SoldPrice * this.Quantity;
            }
        }
    }
}
