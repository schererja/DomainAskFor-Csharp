using System;
using System.Collections.Generic;

namespace DomainAskFor.Models
{
  public class SearchHistoryModel
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string SearchTerm { get; set; }
    public string Prefix { get; set; }
    public string Suffix { get; set; }
    public string TLD { get; set; }
    public DateTime SearchedAt { get; set; }
    public List<SearchResultItem> Results { get; set; }
  }

  public class SearchResultItem
  {
    public string DomainName { get; set; }
    public bool IsAvailable { get; set; }
  }

  public class SaveSearchRequest
  {
    public string SearchTerm { get; set; }
    public string Prefix { get; set; }
    public string Suffix { get; set; }
    public string TLD { get; set; }
    public List<SearchResultItem> Results { get; set; }
  }
}
