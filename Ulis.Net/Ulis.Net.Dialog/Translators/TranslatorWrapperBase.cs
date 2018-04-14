using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Ulis.Net.Library;

namespace Ulis.Net.Dialog.Translators
{
    [Serializable]
    public abstract class TranslatorWrapperBase : ITranslatorClient
    {
        [NonSerialized]
        protected ITranslatorClient _translationClient;

        public async Task<string> TranslateAsync(string text)
        {
            return await _translationClient.TranslateAsync(text);
        }

        [OnDeserialized]
        private void SetValuesOnDeserialized(StreamingContext context)
        {
            SetTranslatorClient();
        }

        // Must be called in each subclass's constructor!
        // The reason why we don't call it in this class's constructor: https://bit.ly/2Hm8XBy
        protected abstract void SetTranslatorClient();
    }
}