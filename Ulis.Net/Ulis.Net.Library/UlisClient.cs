using System.Threading.Tasks;
using Ulis.Net.Library.Luis;

namespace Ulis.Net.Library
{
    public class UlisClient
    {
        private readonly LuisClient _luisClient;
        private readonly ITranslatorClient _translatorClient;

        public UlisClient(ITranslatorClient translatorClient, string luisAppId, string luisSubscriptionKey, string luisRegion)
        {
            _translatorClient = translatorClient;
            _luisClient = new LuisClient(luisAppId, luisSubscriptionKey, luisRegion);
        }

        public async Task<UlisResult> QueryAsync(string text)
        {
            var englishText = await _translatorClient.TranslateAsync(text);
            var luisResult = await _luisClient.QueryAsync(englishText);

            return new UlisResult
            {
                OriginalQuery = text,
                LuisResult = luisResult
            };
        }
    }
}