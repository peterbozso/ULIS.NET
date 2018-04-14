using System;
using Ulis.Net.Dialog.Translators;

namespace Ulis.Net.Dialog.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    [Serializable]
    public class MicrosoftTranslatorAttribute : TranslatorAttributeBase
    {
        public override TranslatorSerializationWrapperBase TranslatorWrapper { get; }

        public MicrosoftTranslatorAttribute(string subscriptionKey)
        {
            TranslatorWrapper = new MicrosoftTranslatorWrapper(subscriptionKey);
        }
    }
}