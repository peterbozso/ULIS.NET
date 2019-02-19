using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ulis.Net.Library
{
    public class MicrosoftTranslatorClient : ITranslatorClient
    {
        private const string MicrosoftTranslatorApiUrlBase = "https://api.microsofttranslator.com/V2/Http.svc/Translate";
        private const string SubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";
        private const string XmlDefaultNamespace = "http://schemas.microsoft.com/2003/10/Serialization/";
        private const string TargetLanguage = "en";

        private readonly string _subscriptionKey;

        public MicrosoftTranslatorClient(string subscriptionKey)
        {
            _subscriptionKey = subscriptionKey;
        }

        public async Task<string> TranslateAsync(string text)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation(SubscriptionKeyHeader, _subscriptionKey);

                var uri = new UriBuilder(MicrosoftTranslatorApiUrlBase)
                {
                    Query = $"text={text}&to={TargetLanguage}"
                }.Uri;

                var translatedXml = await client.GetStringAsync(uri);

                var xmlSerializer = new XmlSerializer(typeof(string), XmlDefaultNamespace);
                using (var reader = new StringReader(translatedXml))
                {
                    return (string)xmlSerializer.Deserialize(reader);
                }
            }
        }
    }
}