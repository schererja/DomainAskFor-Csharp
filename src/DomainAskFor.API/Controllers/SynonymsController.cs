using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using api.models;

namespace DomainAskFor.API.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class SynonymsController : ControllerBase, ISynonymsController
  {
    private string _thesaurusAPIKey;
    private string _merriamWebsterURL;
    private HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly ILogger<SynonymsController> _logger;
    private readonly IDistributedCache _distributedCache;
    private const string SYNONYM_PREFIX = "synonyms";
    private string[] synonyms { get; set; }


    public SynonymsController(ILogger<SynonymsController> logger, IConfiguration configuration, HttpClient httpClient, IDistributedCache distributedCache)
    {
      _logger = logger;
      _config = configuration;
      _httpClient = httpClient;
      _distributedCache = distributedCache;
      _thesaurusAPIKey = _config.GetSection("MerriamWebster").GetSection("ApiKey").Value;
      _merriamWebsterURL = _config.GetSection("MerriamWebster").GetSection("URL").Value;

    }

    [HttpGet("{word}")]
    public async Task<IActionResult> Get(string word)
    {
      var isCached = HttpContext.Request.Query["cache"].ToString();
      var synonyms = new List<string>();
      var cachedSynonyms = new List<string>();
      string url;
      try
      {
        url = _merriamWebsterURL.Replace("{key}", _thesaurusAPIKey).Replace("{word}", word);
      }
      catch (Exception)
      {
        var result = StatusCode(StatusCodes.Status500InternalServerError, "Unable to replace data in URL");
        return result;
      }
      if (isCached == "1")
      {
        cachedSynonyms = GetCachedSynonyms(word);

      }
      if (cachedSynonyms.Count == 0)
      {
        try
        {
          var response = await _httpClient.GetStringAsync(url);
          var jsSerializer = JsonSerializer.Deserialize<List<MerriamWebsterResult>>(response);
          foreach (var synonymsFound in jsSerializer[0].meta.syns)
          {
            foreach (var syn in synonymsFound)
            {
              synonyms.Add(syn);
            }
          }

        }
        catch (InvalidOperationException InvalidOperation)
        {
          var result = StatusCode(StatusCodes.Status500InternalServerError, InvalidOperation.Message);
          return result;
        }
        catch (HttpRequestException HttpRequestException)
        {
          var result = StatusCode(StatusCodes.Status500InternalServerError, HttpRequestException.Message);
          return result;
        }
        catch (TaskCanceledException TaskCanceledException)
        {
          var result = StatusCode(StatusCodes.Status500InternalServerError, TaskCanceledException.Message);
          return result;
        }

        if (SetCachedSynonyms(word, synonyms))
        {
          return Ok(new { synonyms });
        }
        else
        {
          var result = StatusCode(StatusCodes.Status500InternalServerError, "Unable to communicate with Redis Database");
          return result;
        }
      }
      else
      {
        return Ok(new { synonyms = cachedSynonyms });
      }

    }

    private List<string> GetCachedSynonyms(string word)
    {
      var cachedSynonymsList = new List<string>();
      var foundSynonyms = "";
      if (string.IsNullOrEmpty(word))
      {
        return new List<string>();
      }
      try
      {
        foundSynonyms = _distributedCache.GetString(SYNONYM_PREFIX + ":" + word);
      }
      catch (RedisConnectionException RedisConnectionException)
      {
        _logger.LogError($"Unable to connect to Redis: {RedisConnectionException}");
        return new List<string>();

      }
      if (!string.IsNullOrEmpty(foundSynonyms))
      {
        cachedSynonymsList = foundSynonyms.Split(',').ToList();
        return cachedSynonymsList;
      }
      else
      {
        return cachedSynonymsList;
      }

    }
    private bool SetCachedSynonyms(string word, List<string> synonyms)
    {
      if (synonyms.Count == 0 || string.IsNullOrEmpty(word))
      {
        return false;
      }
      try
      {
        _distributedCache.SetString(SYNONYM_PREFIX + ":" + word, string.Join(',', synonyms), new DistributedCacheEntryOptions
        {
          AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300)
        });
        return true;
      }
      catch (Exception)
      {
        return true;
      }

    }

  }

  public interface ISynonymsController
  {
    Task<IActionResult> Get(string word);
  }
}

