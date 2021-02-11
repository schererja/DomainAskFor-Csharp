using DomainAskFor.Models;
using System.Threading.Tasks;

namespace DomainAskFor.Client.HttpRepository.SynonymsRepository
{
    public interface ISynonymRepository
    {
        Task<SynonymsResult> GetSynonymsByWord(string url);
    }
}