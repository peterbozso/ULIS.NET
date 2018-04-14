using System;
using Ulis.Net.Dialog.Translators;

namespace Ulis.Net.Dialog.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    [Serializable]
    public class GoogleTranslatorAttribute : TranslatorAttributeBase
    {
        public override TranslatorSerializationWrapperBase TranslatorWrapper { get; }

        public GoogleTranslatorAttribute(string subscriptionKey)
        {
            TranslatorWrapper = new GoogleTranslatorWrapper(subscriptionKey);
        }
    }
}