using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace EcsDigitalApi.Services
{
    class RelatedWordsService : IRelatedWordsService
    {
        public async Task<IEnumerable<string>> GetRelatedWords(string word)
        {
            var list = await "https://api.datamuse.com/words".SetQueryParam("sl", word).GetJsonListAsync();

            return list.Select(p => (string) p.word);
        }
    }
}