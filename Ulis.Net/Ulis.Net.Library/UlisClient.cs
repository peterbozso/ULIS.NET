using System.Threading.Tasks;

namespace Ulis.Net.Library
{
    public class UlisClient
    {
        private readonly MsTranslatorClient _translatorClient;

        public UlisClient(string msTranslatorSubsriptionKey)
        {
            _translatorClient = new MsTranslatorClient(msTranslatorSubsriptionKey);
        }

        public async Task<string> Translate(string text)
        {
            return await _translatorClient.Translate(text);
        }
    }
}