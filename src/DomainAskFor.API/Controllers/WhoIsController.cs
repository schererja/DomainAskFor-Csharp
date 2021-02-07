using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Whois.NET;

using api.models;

namespace DomainAskFor.API.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class WhoIsController : ControllerBase, IWhoIsController
  {
    private const string AVAILABLE_REGEX = @"(No match for domain\s+(.*)|NOT FOUND\s+(.*))";
    private const string UNAVAILABLE_REGEX = @"/Domain Name:\s+(.*)/g";

    private readonly ILogger<WhoIsController> _logger;

    public WhoIsController(ILogger<WhoIsController> logger)
    {
      _logger = logger;
    }

    [HttpGet("{domainName}")]
    public async Task<IActionResult> Get(string domainName)
    {

      if (Uri.CheckHostName(domainName) != UriHostNameType.Unknown)
      {
        var result = await IsDomainAvailable(domainName);
        var response = new WhoIsResult
        {
          DomainName = domainName,
          IsAvailable = result
        };

        return Ok(response);
      }
      else
      {
        return BadRequest("Invalid domain name");
      }

    }
    private async Task<bool> IsDomainAvailable(string domainName)
    {
      var available_regex = new Regex(AVAILABLE_REGEX);
      var unavailable_regex = new Regex(UNAVAILABLE_REGEX);

      var result = await WhoisClient.QueryAsync(domainName);
      var rawOutput = result.Raw;
      _logger.LogDebug(rawOutput);
      if (available_regex.IsMatch(rawOutput))
      {
        return true;
      }
      else if (unavailable_regex.IsMatch(rawOutput))
      {
        return false;
      }
      return false;

    }

  }
}
