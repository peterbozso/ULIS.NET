using Ulis.Net.Library;

namespace Ulis.Net.Dialog
{
    public class MicrosoftTranslatorWrapper : TranslatorWrapperBase
    {
        protected MicrosoftTranslatorWrapper(string subscriptionKey) : base(subscriptionKey)
        {
            SetTranslatorClient();
        }

        protected override void SetTranslatorClient()
        {
            _translationClient = new MicrosoftTranslatorClient(_subscriptionKey);
        }
    }
}