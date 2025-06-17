using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement;
using OrderManagement.Data;
using OrderManagement.Models;
using OrderManagement.Tests.Factory;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace OrderManagement.Tests.Controllers
{
    public class OrdersControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly ITestOutputHelper _output;

        public OrdersControllerTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _output = output;
        }

        /// <summary>
        /// Intergration Testing the CreateOrder endpoint.
        /// </summary>
        /// <returns></returns>

        [Fact]
        public async Task CreateOrder_ReturnsOk()
        {
            // Arrange
            var payload = new
            {
                CustomerId = 1,
                Items = new[]
                {
                    new
                    {
                        ProductName = "Test Product",
                        Quantity = 2,
                        UnitPrice = 100M
                    }
                }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/orders", payload);

            //helps out debugging by printing the raw response content
            var raw = await response.Content.ReadAsStringAsync();
            _output.WriteLine("Raw response content: " + raw);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CreateOrder_MultipleItems_ReturnsOk_CorrectTotals()
        {

            // Arrange: Multiple items with different prices and quantities
            var payload = new
            {
                CustomerId = 3, //VIP customer (seeded in the database - Program.cs)
                Items = new[]
                {
                    new { ProductName = "Product A", Quantity = 2, UnitPrice = 100M }, // 200
                    new { ProductName = "Product B", Quantity = 1, UnitPrice = 200M }, // 200
                    new { ProductName = "Product C", Quantity = 5, UnitPrice = 50M }   // 250
                }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/orders", payload);

            var raw = await response.Content.ReadAsStringAsync();
            _output.WriteLine("Raw response content: " + raw);

            // Assert: OK response
            response.EnsureSuccessStatusCode();

            // Read the response
            var createdOrder = await response.Content.ReadFromJsonAsync<Order>();
            Assert.NotNull(createdOrder);
            Assert.Equal(3, createdOrder.CustomerId);
            Assert.Equal(3, createdOrder.Items.Count);

            // Expected total before discount
            var expectedTotal = 200M + 200M + 250M; // 650
            Assert.Equal(expectedTotal, createdOrder.TotalAmount + createdOrder.DiscountApplied, precision: 2);

            // Customer 3 is VIP
            Assert.True(createdOrder.DiscountApplied > 0);
            Assert.True(createdOrder.TotalAmount < expectedTotal);
        }



    }
}

