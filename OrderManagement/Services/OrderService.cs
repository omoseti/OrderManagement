using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Data;
using OrderManagement.DTOs;
using OrderManagement.Models;

namespace OrderManagement.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IDiscountService _discountService;

        public OrderService(AppDbContext context, IDiscountService discountService)
        {
            _context = context;
            _discountService = discountService;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            // Load customer to check segment
            var customer = await _context.Customers.FindAsync(order.CustomerId);
            if (customer == null) throw new Exception("Customer not found.");

            // Calculate total amount
            order.TotalAmount = order.Items.Sum(i => i.Quantity * i.UnitPrice);

            // Apply discount
            var discountRate = _discountService.CalculateDiscountRate(customer, order);
            order.DiscountApplied = order.TotalAmount * discountRate;
            order.TotalAmount -= order.DiscountApplied;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }


        public async Task<bool> UpdateStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;

            // Enforce allowed transitions: e.g., can't ship a cancelled order
            if (order.Status == OrderStatus.Cancelled) return false;
            if (newStatus == OrderStatus.Shipped && order.Status != OrderStatus.Confirmed) return false;

            order.Status = newStatus;

            if (newStatus == OrderStatus.Delivered)
            {
                order.FulfilledAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<Order> FulfillOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) throw new Exception("Order not found.");

            if (order.Status != OrderStatus.Pending)
                throw new Exception("Only pending orders can be fulfilled.");

            order.Status = OrderStatus.Fulfilled;
            order.FulfilledAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return order;
        }




        public async Task<OrderAnalyticsDto> GetAnalyticsAsync()
        {
            var fulfilledOrders = await _context.Orders
                 .AsNoTracking() // Use AsNoTracking for read-only queries
                 .Where(o => o.FulfilledAt.HasValue)
                 .ToListAsync();

            var averageValue = await _context.Orders
                 .AsNoTracking() // Use AsNoTracking for read-only queries
                 .AverageAsync(o => o.TotalAmount);


            double averageTime = 0;
            if (fulfilledOrders.Any())
            {
                averageTime = fulfilledOrders
                    .Average(o => (o.FulfilledAt.Value - o.CreatedAt).TotalHours);
            }

            return new OrderAnalyticsDto
            {
                AverageOrderValue = averageValue,
                AverageFulfillmentTimeInHours = averageTime
            };
        }
    }
}
