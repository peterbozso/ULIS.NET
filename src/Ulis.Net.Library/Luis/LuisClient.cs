using Newtonsoft.Json;
using Refit;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ulis.Net.Library.Luis
{
    internal interface ILuisApi
    {
        [Get(@"/{modelId}?subscription-key={subscriptionKey}&staging={staging}
                &verbose={verbose}&timezoneOffset={timezoneOffset}&q={query}")]
        Task<string> Query(string modelId, string subscriptionKey, bool staging, bool verbose,
            int timezoneOffset, string query);
    }

    internal class LuisClient
    {
        private readonly string _domain, _modelId, _subscriptionKey;
        private readonly bool _staging, _verbose;
        private readonly int _timezoneOffset;
        private readonly ILuisApi _luisApi;

        private string LuisApiUrlBase => $"https://{_domain}/luis/v2.0/apps/";

        public LuisClient(HttpClient httpClient, string modelId, string subscriptionKey,
            string domain = "westus.api.cognitive.microsoft.com", bool staging = false,
            bool verbose = false, int timezoneOffset = 0)
        {
            _modelId = modelId;
            _subscriptionKey = subscriptionKey;
            _domain = domain;
            _staging = staging;
            _verbose = verbose;
            _timezoneOffset = timezoneOffset;

            httpClient.BaseAddress = new Uri(LuisApiUrlBase);
            _luisApi = RestService.For<ILuisApi>(httpClient);
        }

        public async Task<LuisResult> QueryAsync(string query)
        {
            return JsonConvert.DeserializeObject<LuisResult>(
                await _luisApi.Query(_modelId, _subscriptionKey, _staging, _verbose, _timezoneOffset, query));
        }
    }
}