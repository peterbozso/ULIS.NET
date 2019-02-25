using System.Threading.Tasks;
using Google.Cloud.Translation.V2;

namespace Ulis.Net.Recognizer.Translation
{
    public class GoogleTranslatorClient : ITranslatorClient
    {
        private readonly string _sourceLangugage;
        private readonly TranslationClient _translationClient;

        public GoogleTranslatorClient(string subscriptionKey, string sourceLangugage)
        {
            _translationClient = TranslationClient.CreateFromApiKey(subscriptionKey);
            _sourceLangugage = sourceLangugage;
        }

        public async Task<string> TranslateAsync(string text)
        {
            var result = await _translationClient.TranslateTextAsync(
                text, LanguageCodes.English, _sourceLangugage).ConfigureAwait(false);
            return result.TranslatedText;
        }
    }
}