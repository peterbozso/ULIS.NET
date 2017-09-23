using System.Threading.Tasks;
using Google.Cloud.Translation.V2;

namespace Ulis.Net.Library
{
    class GoogleTranslatorClient : ITranslatorClient
    {
        private readonly TranslationClient _translationClient;

        public GoogleTranslatorClient(string subscriptionKey)
        {
            _translationClient = TranslationClient.CreateFromApiKey(subscriptionKey);
        }

        public async Task<string> Translate(string text)
        {
            var result = await _translationClient.TranslateTextAsync(text, LanguageCodes.English);
            return result.TranslatedText;
        }
    }
}