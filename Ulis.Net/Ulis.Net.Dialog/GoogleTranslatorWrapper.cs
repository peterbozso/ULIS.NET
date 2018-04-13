using Ulis.Net.Library;

namespace Ulis.Net.Dialog
{
    public class GoogleTranslatorWrapper : TranslatorWrapperBase
    {
        protected GoogleTranslatorWrapper(string subscriptionKey) : base(subscriptionKey)
        {
            SetTranslatorClient();
        }

        protected override void SetTranslatorClient()
        {
            _translationClient = new GoogleTranslatorClient(_subscriptionKey);
        }
    }
}