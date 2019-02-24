using System.Threading.Tasks;
using Google.Cloud.Translation.V2;

namespace Ulis.Net.Recognizer.Translation
{
    public class GoogleTranslatorClient : ITranslatorClient
    {
        private readonly TranslationClient _translationClient;

        public GoogleTranslatorClient(string subscriptionKey)
        {
            _translationClient = TranslationClient.CreateFromApiKey(subscriptionKey);
        }

        public async Task<string> TranslateAsync(string text)
        {
            var result = await _translationClient.TranslateTextAsync(text, LanguageCodes.English);
            return result.TranslatedText;
        }
    }
}