using System;
using Ulis.Net.Library;

namespace Ulis.Net.Dialog.Translators
{
    [Serializable]
    public class MicrosoftTranslatorWrapper : TranslatorWrapperBase
    {
        private readonly string _subscriptionKey;

        public MicrosoftTranslatorWrapper(string subscriptionKey)
        {
            _subscriptionKey = subscriptionKey;
            SetTranslatorClient();
        }

        protected override void SetTranslatorClient()
        {
            _translationClient = new MicrosoftTranslatorClient(_subscriptionKey);
        }
    }
}