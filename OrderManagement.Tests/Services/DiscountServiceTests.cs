using OrderManagement.Models;
using OrderManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Tests.Services
{
    public class DiscountServiceTests
    {
        private readonly DiscountService _service;

        public DiscountServiceTests()
        {
            _service = new DiscountService();
        }

        [Theory]
        [InlineData(CustomerSegment.Regular, 100, 0.00)]
        [InlineData(CustomerSegment.Wholesale, 100, 0.05)]
        [InlineData(CustomerSegment.VIP, 100, 0.10)]
        [InlineData(CustomerSegment.VIP, 2000, 0.15)] // VIP + general big order only
        [InlineData(CustomerSegment.VIP, 5000, 0.15)] // still only 15%
        [InlineData(CustomerSegment.Wholesale, 2000, 0.10)] // Wholesale + big order
        [InlineData(CustomerSegment.Regular, 2000, 0.05)] // Regular + big order
        public void CalculateDiscountRate_ReturnsExpectedRate(CustomerSegment segment, decimal amount, decimal expectedRate)
        {
            var customer = new Customer { Segment = segment };
            var order = new Order { TotalAmount = amount };

            var rate = _service.CalculateDiscountRate(customer, order);

            Assert.Equal(expectedRate, rate);
        }

        [Theory]
        [InlineData(CustomerSegment.VIP, 1000, 100)]  // 10% -> $100
        [InlineData(CustomerSegment.VIP, 5000, 750)] // 15% -> $750
        [InlineData(CustomerSegment.Wholesale, 500, 25)] // 5% -> $25
        public void CalculateDiscountAmount_ReturnsExpectedAmount(CustomerSegment segment, decimal amount, decimal expectedAmount)
        {
            var customer = new Customer { Segment = segment };
            var order = new Order { TotalAmount = amount };

            var discountAmount = _service.CalculateDiscountAmount(customer, order);

            Assert.Equal(expectedAmount, discountAmount);
        }
    }
}
