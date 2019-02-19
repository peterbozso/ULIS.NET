using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ulis.Net.Library
{
    internal interface IMicrosoftTranslatorApi
    {
        [Post(@"/translate?api-version=3.0&to={targetLanguage}")]
        Task<string> Translate(string targetLanguage, [Body] MicrosoftTranslatorText[] text);
    }

    internal class MicrosoftTranslatorText
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }

    public class MicrosoftTranslatorClient : ITranslatorClient
    {
        private class MicrosoftTranslatorResult
        {
            [JsonProperty(PropertyName = "translations")]
            public List<MicrosoftTranslatorText> Translations { get; set; }
        }

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
            var jsonResult = await _microsoftTranslatorApi.Translate(TargetLanguage,
                new [] { new MicrosoftTranslatorText { Text = text } });
            var translatorResult = JsonConvert.DeserializeObject<MicrosoftTranslatorResult[]>(jsonResult);
            return translatorResult[0].Translations[0].Text;
        }
    }
}