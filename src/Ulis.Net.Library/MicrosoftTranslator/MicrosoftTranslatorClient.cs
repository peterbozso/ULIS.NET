using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Refit;

namespace Ulis.Net.Library.MicrosoftTranslator
{
    public partial class MicrosoftTranslatorClient : ITranslatorClient
    {
        private const string MicrosoftTranslatorApiUrlBase = "https://api.cognitive.microsofttranslator.com/";
        private const string SubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";
        private const string TargetLanguage = "en";

        private readonly IMicrosoftTranslatorApi _microsoftTranslatorApi;

        public MicrosoftTranslatorClient(HttpClient httpClient, string subscriptionKey)
        {
            httpClient.BaseAddress = new Uri(MicrosoftTranslatorApiUrlBase);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation(SubscriptionKeyHeader, subscriptionKey);
            _microsoftTranslatorApi = RestService.For<IMicrosoftTranslatorApi>(httpClient);
        }

        public async Task<string> TranslateAsync(string text)
        {
            var jsonResult = await _microsoftTranslatorApi.Translate(
                TargetLanguage,
                new[] { new MicrosoftTranslatorText { Text = text } });
            var translatorResult = JsonConvert.DeserializeObject<MicrosoftTranslatorResult[]>(jsonResult);
            return translatorResult[0].Translations[0].Text;
        }
    }
}