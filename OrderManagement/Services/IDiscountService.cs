using OrderManagement.Models;

namespace OrderManagement.Services
{
    public interface IDiscountService
    {
        decimal CalculateDiscountRate(Customer customer, Order order);
        decimal CalculateDiscountAmount(Customer customer, Order order);
    }
}
