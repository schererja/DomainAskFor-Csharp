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
        Console.WriteLine($"Attempting to save search for term: {request.SearchTerm}");
        var response = await _httpClient.PostAsJsonAsync("api/v1/SearchHistory", request);
        Console.WriteLine($"Save search response status: {response.StatusCode}");
        if (!response.IsSuccessStatusCode)
        {
          var errorContent = await response.Content.ReadAsStringAsync();
          Console.WriteLine($"Save search error: {errorContent}");
        }
        return response.IsSuccessStatusCode;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Exception saving search: {ex.Message}");
        return false;
      }
    }

    public async Task<List<SearchHistoryModel>> GetSearchHistory()
    {
      try
      {
        Console.WriteLine("Attempting to get search history...");
        var response = await _httpClient.GetFromJsonAsync<List<SearchHistoryModel>>("api/v1/SearchHistory");
        Console.WriteLine($"Retrieved {response?.Count ?? 0} search history items");
        return response ?? new List<SearchHistoryModel>();
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Exception getting search history: {ex.Message}");
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
