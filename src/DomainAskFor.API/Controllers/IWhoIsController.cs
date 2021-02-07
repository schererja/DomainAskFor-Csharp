using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DomainAskFor.API.Controllers
{
    public interface IWhoIsController
    {
        Task<IActionResult> Get(string domainName);
    }
}