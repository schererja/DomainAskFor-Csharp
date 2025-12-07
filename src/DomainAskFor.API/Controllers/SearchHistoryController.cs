using DomainAskFor.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainAskFor.API.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class SearchHistoryController : ControllerBase
  {
    // In-memory storage for demo purposes
    // In production, use a proper database
    private static List<SearchHistoryModel> _searchHistory = new List<SearchHistoryModel>();
    private static int _nextId = 1;

    [HttpPost]
    public async Task<ActionResult> SaveSearch([FromBody] SaveSearchRequest request)
    {
      try
      {
        // In a real app, you'd get the user ID from the JWT token
        // For now, we'll use a default user ID of 1
        var search = new SearchHistoryModel
        {
          Id = _nextId++,
          UserId = 1, // Should come from authenticated user
          SearchTerm = request.SearchTerm,
          Prefix = request.Prefix,
          Suffix = request.Suffix,
          TLD = request.TLD,
          SearchedAt = DateTime.UtcNow,
          Results = request.Results
        };

        _searchHistory.Add(search);
        return Ok();
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpGet]
    public async Task<ActionResult<List<SearchHistoryModel>>> GetSearchHistory()
    {
      try
      {
        // In a real app, filter by authenticated user ID
        // For now, return all searches for user ID 1
        var history = _searchHistory.Where(s => s.UserId == 1).ToList();
        return Ok(history);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSearch(int id)
    {
      try
      {
        var search = _searchHistory.FirstOrDefault(s => s.Id == id);
        if (search != null)
        {
          _searchHistory.Remove(search);
          return Ok();
        }
        return NotFound();
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}
