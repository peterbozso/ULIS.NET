using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Refit;

namespace Ulis.Net.Library
{
    interface IMsTranslatorApi
    {
        [Get("/Translate?text={text}&to={to}")]
        Task<string> Translate(string text, string to, [Header("Ocp-Apim-Subscription-Key")] string subscriptionKey);
    }

    class MsTranslatorClient : ITranslatorClient
    {
        private const string MsTranslatorApiUrlBase = "https://api.microsofttranslator.com/V2/Http.svc/";
        private const string XmlDefaultNamespace = "http://schemas.microsoft.com/2003/10/Serialization/";
        private const string TargetLanguage = "en";

        private readonly IMsTranslatorApi _msTranslatorApi;
        private readonly string _subscriptionKey;

        public MsTranslatorClient(string subscriptionKey)
        {
            _subscriptionKey = subscriptionKey;
            _msTranslatorApi = RestService.For<IMsTranslatorApi>(MsTranslatorApiUrlBase);
        }

        public async Task<string> Translate(string text)
        {
            var translatedXml = await _msTranslatorApi.Translate(text, TargetLanguage, _subscriptionKey);

            var xmlSerializer = new XmlSerializer(typeof(string), XmlDefaultNamespace);
            using (var reader = new StringReader(translatedXml))
            {
                return (string) xmlSerializer.Deserialize(reader);
            }
        }
    }
}