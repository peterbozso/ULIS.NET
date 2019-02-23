using System.Threading.Tasks;
using Refit;

namespace Ulis.Net.Library.Luis
{
    internal interface ILuisApi
    {
        [Get(@"/{modelId}?subscription-key={subscriptionKey}&staging={staging}
                &verbose={verbose}&timezoneOffset={timezoneOffset}&q={query}")]
        Task<string> Query(string modelId, string subscriptionKey, bool staging, bool verbose, int timezoneOffset, string query);
    }
}