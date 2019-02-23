using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Refit;

namespace Ulis.Net.Library.Luis
{
    internal class LuisClient
    {
        private readonly string _domain, _modelId, _subscriptionKey;
        private readonly ILuisApi _luisApi;
        private readonly bool _staging, _verbose;
        private readonly int _timezoneOffset;

        public LuisClient(HttpClient httpClient, string modelId, string subscriptionKey, string domain = "westus.api.cognitive.microsoft.com", bool staging = false, bool verbose = false, int timezoneOffset = 0)
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

        private string LuisApiUrlBase => $"https://{_domain}/luis/v2.0/apps/";

        public async Task<LuisResult> QueryAsync(string query)
        {
            return JsonConvert.DeserializeObject<LuisResult>(
                await _luisApi.Query(_modelId, _subscriptionKey, _staging, _verbose, _timezoneOffset, query));
        }
    }
}