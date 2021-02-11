
using DomainAskFor.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DomainAskFor.Client.HttpRepository
{
    public class WhoIsRepository : IWhoIsHttpRepository
    {
        private readonly HttpClient _client;

        public WhoIsRepository(HttpClient client)
        {
            _client = client;
        }
        public async Task<WhoIsResult> GetWhoIsResultByURI(string url)
        {
            var response = await _client.GetAsync($"https://localhost:44314/api/v1/WhoIs/{url}");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var whoIsResponse = JsonSerializer.Deserialize<WhoIsResult>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return whoIsResponse;
        }

    }
}
