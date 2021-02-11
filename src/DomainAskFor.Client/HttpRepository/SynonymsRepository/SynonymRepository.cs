using DomainAskFor.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DomainAskFor.Client.HttpRepository.SynonymsRepository
{
    public class SynonymRepository : ISynonymRepository
    {
        private readonly HttpClient _client;

        public SynonymRepository(HttpClient client)
        {
            _client = client;
        }
        public async Task<SynonymsResult> GetSynonymsByWord(string word)
        {
            var response = await _client.GetAsync($"https://localhost:49155/api/v1/Synonyms/{word}?cache=1");
            if (!response.IsSuccessStatusCode)
            {
                return new SynonymsResult();
            }
            var content = await response.Content.ReadAsStringAsync();

            var synonymsResult = JsonSerializer.Deserialize<SynonymsResult>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            foreach (var foundSynonym in synonymsResult.synonyms)
            {
                Console.WriteLine(foundSynonym);
            }
            return synonymsResult;
        }
    }
}
