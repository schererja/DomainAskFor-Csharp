using domainaskfor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainAskFor.Client.HttpRepository
{
    public interface IWhoIsHttpRepository
    {
        Task<WhoIsResult> GetWhoIsResultByURI(string url);
    }
}
