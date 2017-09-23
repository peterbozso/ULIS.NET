using System;
using System.Threading.Tasks;
using Microsoft.Cognitive.LUIS;

namespace Ulis.Net.Library
{
    public class UlisClient
    {
        private readonly LuisClient _luisClient;
        private readonly ITranslatorClient _translatorClient;

        public UlisClient(TranslatorClients translatorClient, string translatorSubsriptionKey,
            string luisAppId, string luisAppKey)
        {
            switch (translatorClient)
            {
                case TranslatorClients.Microsoft:
                    _translatorClient = new MicrosoftTranslatorClient(translatorSubsriptionKey);
                    break;
                case TranslatorClients.Google:
                    _translatorClient = new GoogleTranslatorClient(translatorSubsriptionKey);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(translatorClient));
            }

            _luisClient = new LuisClient(luisAppId, luisAppKey);
        }

        public async Task<LuisResult> QueryAsync(string text)
        {
            var englishText = await _translatorClient.TranslateAsync(text);
            return await _luisClient.Predict(englishText);
        }
    }
}