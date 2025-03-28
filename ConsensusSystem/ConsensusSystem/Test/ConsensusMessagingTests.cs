using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace ConsensusSystem.Test
{
    public class ConsensusMessagingTests
    {
        private readonly HttpClient _httpClient = new HttpClient();

        [Fact]
        public async Task SendMessage_ShouldReceiveMessage()
        {
            // Arrange
            var senderNodeUrl = "http://localhost:5001";
            var receiverNodeUrl = "http://localhost:5002";

            var message = new { SenderId = 1, ReceiverId = 2, Content = "Test Message" };
            var jsonContent = new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");

            // Act
            var response = await _httpClient.PostAsync($"{receiverNodeUrl}/receive", jsonContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Message received", responseContent);
        }

        [Fact]
        public async Task BroadcastMessage_ShouldBeReceivedByAllNodes()
        {
            // Arrange
            var senderNodeUrl = "http://localhost:5001";
            var nodes = new[] { "http://localhost:5002", "http://localhost:5003" };

            var message = new { SenderId = 1, Content = "Broadcast Test" };
            var jsonContent = new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");

            foreach (var node in nodes)
            {
                // Act
                var response = await _httpClient.PostAsync($"{node}/receive", jsonContent);

                // Assert
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                Assert.Contains("Message received", responseContent);
            }
        }
    }
}
