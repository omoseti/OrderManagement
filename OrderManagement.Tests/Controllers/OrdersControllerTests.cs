using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using OrderManagement;

namespace OrderManagement.Tests.Controllers
{
    public class OrdersControllerTests : IClassFixture<WebApplicationFactory<OrderManagement.Program>>
    {
        private readonly HttpClient _client;

        public OrdersControllerTests(WebApplicationFactory<OrderManagement.Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateOrder_ReturnsOk()
        {
            // Arrange: valid test order
            var order = new
            {
                CustomerId = 1,
                Amount = 100
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/orders", order);

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
