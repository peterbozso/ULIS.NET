using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Refit;

namespace Ulis.Net.Library
{
    interface IMicrosoftTranslatorApi
    {
        [Get("/Translate?text={text}&to={to}")]
        Task<string> Translate(string text, string to, [Header("Ocp-Apim-Subscription-Key")] string subscriptionKey);
    }

    public class MicrosoftTranslatorClient : ITranslatorClient
    {
        private const string MicrosoftTranslatorApiUrlBase = "https://api.microsofttranslator.com/V2/Http.svc/";
        private const string XmlDefaultNamespace = "http://schemas.microsoft.com/2003/10/Serialization/";
        private const string TargetLanguage = "en";

        private readonly IMicrosoftTranslatorApi _microsoftTranslatorApi;
        private readonly string _subscriptionKey;

        public MicrosoftTranslatorClient(string subscriptionKey)
        {
            _subscriptionKey = subscriptionKey;
            _microsoftTranslatorApi = RestService.For<IMicrosoftTranslatorApi>(MicrosoftTranslatorApiUrlBase);
        }

        public async Task<string> TranslateAsync(string text)
        {
            var translatedXml = await _microsoftTranslatorApi.Translate(text, TargetLanguage, _subscriptionKey);

            var xmlSerializer = new XmlSerializer(typeof(string), XmlDefaultNamespace);
            using (var reader = new StringReader(translatedXml))
            {
                return (string) xmlSerializer.Deserialize(reader);
            }
        }
    }
}