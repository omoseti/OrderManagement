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

        //[Theory]
        //[InlineData(CustomerSegment.Regular, 100, 0)]
        //[InlineData(CustomerSegment.Wholesale, 100, 10)]
        //[InlineData(CustomerSegment.VIP, 100, 20)]
        //public void CalculateDiscount_AppliesCorrectPercentage(CustomerSegment segment, decimal amount, decimal expected)
        //{
        //    var customer = new Customer { Segment = segment };
        //    var order = new Order { TotalAmount = amount };

        //    var discount = _service.CalculateDiscountRate(customer, order);

        //    Assert.Equal(expected, discount);
        //}

        [Theory]
        [InlineData(CustomerSegment.Regular, 100, 0.00)]
        [InlineData(CustomerSegment.Wholesale, 100, 0.05)]
        [InlineData(CustomerSegment.VIP, 100, 0.10)]
        [InlineData(CustomerSegment.VIP, 2000, 0.15)] // VIP + big amount
        [InlineData(CustomerSegment.VIP, 5000, 0.20)] // capped at 20%
        public void CalculateDiscountRate_ReturnsExpectedRate(CustomerSegment segment, decimal amount, decimal expectedRate)
        {
            var customer = new Customer { Segment = segment };
            var order = new Order { TotalAmount = amount };

            var rate = _service.CalculateDiscountRate(customer, order);

            Assert.Equal(expectedRate, rate);
        }

        [Theory]
        [InlineData(CustomerSegment.VIP, 1000, 100)]  // 10% -> $100
        [InlineData(CustomerSegment.VIP, 5000, 1000)] // capped at 20% -> $1000
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
