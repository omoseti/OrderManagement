using OrderManagement.Models;

namespace OrderManagement.Services
{
    public class DiscountService : IDiscountService
    {
        public decimal CalculateDiscountRate(Customer customer, Order order)
        {
            decimal rate = 0m;

            if (customer.Segment == CustomerSegment.VIP)
            {
                rate += 0.10m; // VIP = +10%
            }
            else if (customer.Segment == CustomerSegment.Wholesale)
            {
                rate += 0.05m; // Wholesale = +5%
            }

            if (order.TotalAmount > 1000)
            {
                rate += 0.05m; // Everyone gets +5% for big orders
            }

            if (rate > 0.20m) rate = 0.20m;

            return rate;
        }





        /// <summary>
        /// Returns the discount amount in money, e.g. KES 200.
        /// </summary>
        public decimal CalculateDiscountAmount(Customer customer, Order order)
        {
            var rate = CalculateDiscountRate(customer, order);
            return order.TotalAmount * rate;
        }
    }

}
