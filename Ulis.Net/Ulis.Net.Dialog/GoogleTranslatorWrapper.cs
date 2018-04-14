using System;
using Ulis.Net.Library;

namespace Ulis.Net.Dialog
{
    [Serializable]
    public class GoogleTranslatorWrapper : TranslatorWrapperBase
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