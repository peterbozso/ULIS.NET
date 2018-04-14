using System;
using Ulis.Net.Library;

namespace Ulis.Net.Dialog.Translators
{
    [Serializable]
    public class GoogleTranslatorWrapper : TranslatorSerializationWrapperBase
    {
        private readonly string _subscriptionKey;

        public GoogleTranslatorWrapper(string subscriptionKey)
        {
            _subscriptionKey = subscriptionKey;
            SetTranslatorClient();
        }

        protected override void SetTranslatorClient()
        {
            _translationClient = new GoogleTranslatorClient(_subscriptionKey);
        }
    }
}