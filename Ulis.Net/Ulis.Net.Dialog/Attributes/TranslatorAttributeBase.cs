using System;
using Ulis.Net.Dialog.Translators;

namespace Ulis.Net.Dialog.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    [Serializable]
    public abstract class TranslatorAttributeBase : Attribute
    {
        public abstract TranslatorSerializationWrapperBase TranslatorWrapper { get; }
    }
}
