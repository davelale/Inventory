using Xunit;
using System;
using Inventory.Api.IntegrationTests.InventoryApi;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net;
using System.Threading.Tasks;

namespace Inventory.Api.IntegrationTests
{
    public class TestStockApi
    {
        private Client _client;

        public TestStockApi()
        {
            var webApplicationFactory = new TestWebApplicationFactory<Startup>();
            var httpClient = webApplicationFactory.CreateClient();
            _client = new Client(httpClient.BaseAddress.AbsoluteUri, httpClient);
        }

        [Fact]
        public async Task GivenWeAddStockThenItCanBeRetrieved()
        {
            var stockName = GetRandomName();
            Stock newStock = new Stock() { Description=stockName, NumberInStock=10, DeliveryDate=DateTime.Now.AddDays(10), NumberToBeDelivered=100 };

            var addedStock = await _client.ApiStocksPostAsync(newStock);

            var existingStock = await _client.ApiStocksGetAsync(newStock.Description);
            Assert.Equal(newStock.Description, existingStock.Description);
        }

        [Fact]
        public async Task NotExistantStockReturnsAnError()
        {
            var stockName = GetRandomName();
            var exception = await Assert.ThrowsAsync<ApiException>(() => _client.ApiStocksGetAsync(stockName));
            Assert.Equal(HttpStatusCode.NotFound, (HttpStatusCode)exception.StatusCode);

            ProblemDetails problemDetails = JsonSerializer.Deserialize<ProblemDetails>(exception.Response);
            Assert.Equal(HttpStatusCode.NotFound, (HttpStatusCode)problemDetails.Status);
        }

        [Fact]
        public async Task BadStockDeliveryQuantityReturnsAnError()
        {
            var stockName = GetRandomName();
            Stock newStock = new Stock() { Description = stockName, NumberInStock = 10, DeliveryDate = DateTime.Now.AddDays(10), NumberToBeDelivered = -1 };

            var exception = await Assert.ThrowsAsync<ApiException>(() => _client.ApiStocksPostAsync(newStock));
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)exception.StatusCode);

            ProblemDetails problemDetails = JsonSerializer.Deserialize<ProblemDetails>(exception.Response);
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)problemDetails.Status);
        }

        [Fact]
        public async Task UpdateWhenStock()
        {
            var stockName = GetRandomName();
            Stock newStock = new Stock() { Description = stockName, NumberInStock = 10, DeliveryDate = DateTime.Now.AddDays(10), NumberToBeDelivered = 100 };

            var addedStock = await _client.ApiStocksPostAsync(newStock);

            var existingStock = await _client.ApiStocksGetAsync(newStock.Description);
            existingStock.NumberInStock -= 2;
            await _client.ApiStocksPutAsync(existingStock.Description, existingStock);

            var updatedStock = await _client.ApiStocksGetAsync(existingStock.Description);
            Assert.Equal(8, updatedStock.NumberInStock);
        }

        [Fact]
        public async Task UpdateWhenNotEnoughStock()
        {
            var stockName = GetRandomName();
            Stock newStock = new Stock() { Description = stockName, NumberInStock = 10, DeliveryDate = DateTime.Now.AddDays(10), NumberToBeDelivered = 100 };

            var addedStock = await _client.ApiStocksPostAsync(newStock);

            var existingStock = await _client.ApiStocksGetAsync(newStock.Description);
            existingStock.NumberInStock -= 11;
            var exception = await Assert.ThrowsAsync<ApiException>(() => _client.ApiStocksPutAsync(existingStock.Description, existingStock));
        }

        private string GetRandomName()
        {
            return Guid.NewGuid().ToString("N")
                .ToUpper()
                .Replace("0", "G")
                .Replace("1", "H")
                .Replace("2", "I")
                .Replace("3", "J")
                .Replace("4", "K")
                .Replace("5", "L")
                .Replace("6", "M")
                .Replace("7", "N")
                .Replace("8", "O")
                .Replace("9", "P");
        }
    }
}
