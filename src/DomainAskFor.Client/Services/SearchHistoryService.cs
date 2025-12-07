using DomainAskFor.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DomainAskFor.Client.Services
{
  public interface ISearchHistoryService
  {
    Task<bool> SaveSearch(SaveSearchRequest request);
    Task<List<SearchHistoryModel>> GetSearchHistory();
    Task<bool> DeleteSearch(int searchId);
  }

  public class SearchHistoryService : ISearchHistoryService
  {
    private readonly HttpClient _httpClient;

    public SearchHistoryService(HttpClient httpClient)
    {
      _httpClient = httpClient;
    }

    public async Task<bool> SaveSearch(SaveSearchRequest request)
    {
      try
      {
        var response = await _httpClient.PostAsJsonAsync("api/v1/SearchHistory", request);
        return response.IsSuccessStatusCode;
      }
      catch
      {
        return false;
      }
    }

    public async Task<List<SearchHistoryModel>> GetSearchHistory()
    {
      try
      {
        var response = await _httpClient.GetFromJsonAsync<List<SearchHistoryModel>>("api/v1/SearchHistory");
        return response ?? new List<SearchHistoryModel>();
      }
      catch
      {
        return new List<SearchHistoryModel>();
      }
    }

    public async Task<bool> DeleteSearch(int searchId)
    {
      try
      {
        var response = await _httpClient.DeleteAsync($"api/v1/SearchHistory/{searchId}");
        return response.IsSuccessStatusCode;
      }
      catch
      {
        return false;
      }
    }
  }
}
