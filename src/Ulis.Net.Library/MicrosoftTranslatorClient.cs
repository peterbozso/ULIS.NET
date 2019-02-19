using Refit;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ulis.Net.Library
{
    internal interface IMicrosoftTranslatorApi
    {
        [Get(@"/Translate?text={text}&to={targetLanguage}")]
        Task<string> Translate(string text, string targetLanguage);
    }

    public class MicrosoftTranslatorClient : ITranslatorClient
    {
        private const string MicrosoftTranslatorApiUrlBase = "https://api.microsofttranslator.com/V2/Http.svc/";
        private const string SubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";
        private const string XmlDefaultNamespace = "http://schemas.microsoft.com/2003/10/Serialization/";
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
            var translatedXml = await _microsoftTranslatorApi.Translate(text, TargetLanguage);

            var xmlSerializer = new XmlSerializer(typeof(string), XmlDefaultNamespace);
            using (var reader = new StringReader(translatedXml))
            {
                return (string)xmlSerializer.Deserialize(reader);
            }
        }
    }
}