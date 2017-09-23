using System.Threading.Tasks;
using Microsoft.Cognitive.LUIS;

namespace Ulis.Net.Library
{
    public class UlisClient
    {
        private readonly LuisClient _luisClient;
        private readonly ITranslatorClient _translatorClient;

        public UlisClient(ITranslatorClient translatorClient, string luisAppId, string luisAppKey)
        {
            _translatorClient = translatorClient;
            _luisClient = new LuisClient(luisAppId, luisAppKey);
        }

        public async Task<LuisResult> QueryAsync(string text)
        {
            var englishText = await _translatorClient.TranslateAsync(text);
            return await _luisClient.Predict(englishText);
        }
    }
}