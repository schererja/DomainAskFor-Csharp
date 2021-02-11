
using DomainAskFor.Models;
using System.Threading.Tasks;

namespace DomainAskFor.Client.HttpRepository
{
    public interface IWhoIsHttpRepository
    {
        Task<WhoIsResult> GetWhoIsResultByURI(string url);
    }
}
