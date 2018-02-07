using Newtonsoft.Json;
using Refit;
using System.Threading.Tasks;

namespace Ulis.Net.Library.Luis
{
    internal interface ILuisApi
    {
        [Get(@"/{appId}?subscription-key={subscriptionKey}&staging={staging}
                &verbose={verbose}&timezoneOffset={timezoneOffset}&q={query}")]
        Task<string> Query(string appId, string subscriptionKey, bool staging, bool verbose,
            int timezoneOffset, string query);
    }

    internal class LuisClient
    {
        private readonly string _region, _appId, _subscriptionKey;
        private readonly bool _staging, _verbose;
        private readonly int _timezoneOffset;
        private readonly ILuisApi _luisApi;

        private string LuisApiUrlBase => $"https://{_region}.api.cognitive.microsoft.com/luis/v2.0/apps/";

        public LuisClient(string appId, string subscriptionKey, string region = "westus",
            bool staging = false, bool verbose = false, int timezoneOffset = 0)
        {
            _appId = appId;
            _subscriptionKey = subscriptionKey;
            _region = region;
            _staging = staging;
            _verbose = verbose;
            _timezoneOffset = timezoneOffset;

            _luisApi = RestService.For<ILuisApi>(LuisApiUrlBase);
        }

        public async Task<LuisResult> QueryAsync(string query)
        {
            return JsonConvert.DeserializeObject<LuisResult>(
                await _luisApi.Query(_appId, _subscriptionKey, _staging, _verbose, _timezoneOffset, query));
        }
    }
}