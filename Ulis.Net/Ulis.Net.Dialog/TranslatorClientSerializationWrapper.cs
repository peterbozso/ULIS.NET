using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Ulis.Net.Library;

namespace Ulis.Net.Dialog
{
    /// <summary>
    /// Needed because of this: https://github.com/dotnet/standard/issues/300
    /// Essentially our ITranslatorClient implementations cannot be made Serializable, so we wrap them this way.
    /// </summary>
    [Serializable]
    public class TranslatorClientSerializationWrapper : ITranslatorClient
    {
        [NonSerialized]
        private ITranslatorClient _translationClient;

        private readonly TranslationProvider _translationProvider;
        private readonly string _subscriptionKey;

        public TranslatorClientSerializationWrapper(TranslationProvider translationProvider, string subscriptionKey)
        {
            _translationProvider = translationProvider;
            _subscriptionKey = subscriptionKey;
            SetTranslatorClient();
        }

        public async Task<string> TranslateAsync(string text)
        {
            return await _translationClient.TranslateAsync(text);
        }

        [OnDeserialized]
        private void SetValuesOnDeserialized(StreamingContext context)
        {
            SetTranslatorClient();
        }

        private void SetTranslatorClient()
        {
            switch (_translationProvider)
            {
                case TranslationProvider.Microsoft:
                    _translationClient = new MicrosoftTranslatorClient(_subscriptionKey);
                    break;

                case TranslationProvider.Google:
                    _translationClient = new GoogleTranslatorClient(_subscriptionKey);
                    break;
            }
        }
    }
}