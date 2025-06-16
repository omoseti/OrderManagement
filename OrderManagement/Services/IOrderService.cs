using OrderManagement.DTOs;
using OrderManagement.Models;

namespace OrderManagement.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<bool> UpdateStatusAsync(int orderId, OrderStatus newStatus);
        Task<OrderAnalyticsDto> GetAnalyticsAsync();
    }
}
