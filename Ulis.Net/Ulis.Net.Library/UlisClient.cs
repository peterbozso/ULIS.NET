using System;
using System.Threading.Tasks;

namespace Ulis.Net.Library
{
    public class UlisClient
    {
        private readonly ITranslatorClient _translatorClient;

        public UlisClient(TranslatorClients translatorClient, string subsriptionKey)
        {
            switch (translatorClient)
            {
                case TranslatorClients.Microsoft:
                    _translatorClient = new MicrosoftTranslatorClient(subsriptionKey);
                    break;
                case TranslatorClients.Google:
                    _translatorClient = new GoogleTranslatorClient(subsriptionKey);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(translatorClient));
            }
        }

        public async Task<string> Translate(string text)
        {
            return await _translatorClient.Translate(text);
        }
    }
}