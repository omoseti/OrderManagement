using OrderManagement.Models;

namespace OrderManagement.Services
{
    public class DiscountService : IDiscountService
    {
        /// <summary>
        /// Returns the discount rate as a decimal fraction. E.g. 0.10 means 10% off.
        /// </summary>
        public decimal CalculateDiscountRate(Customer customer, Order order)
        {
            decimal rate = 0m;

            if (customer.Segment == CustomerSegment.VIP)
            {
                rate += 0.10m; // 10% for VIP
            }
            else if (customer.Segment == CustomerSegment.Wholesale)
            {
                rate += 0.05m; // 5% for Wholesale
            }

            if (order.TotalAmount > 1000)
            {
                rate += 0.05m; // Extra 5% for big spenders
            }

            // Cap at 20%
            if (rate > 0.20m) rate = 0.20m;

            return rate;
        }

        /// <summary>
        /// Returns the discount amount in money, e.g. $200.
        /// </summary>
        public decimal CalculateDiscountAmount(Customer customer, Order order)
        {
            var rate = CalculateDiscountRate(customer, order);
            return order.TotalAmount * rate;
        }
    }

}
