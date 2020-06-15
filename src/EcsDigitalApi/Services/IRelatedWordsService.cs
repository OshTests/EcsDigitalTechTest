using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcsDigitalApi.Services
{
    public interface IRelatedWordsService
    {
        Task<IEnumerable<string>> GetRelatedWords(string word);
    }
}