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

        public async Task<LuisResult> QueryAsync(string text)
        {
            var englishText = await _translatorClient.TranslateAsync(text);
            return await _luisClient.QueryAsync(englishText);
        }
    }
}