using System;

namespace Ulis.Net.Dialog
{
    [AttributeUsage(AttributeTargets.Class)]
    [Serializable]
    public class UlisTranslatorAttribute : Attribute
    {
        public TranslatorWrapperBase TranslatorWrapper { get; }

        public UlisTranslatorAttribute(Type translatorWrapperType, params object[] parameters)
        {
            TranslatorWrapper = Activator.CreateInstance(translatorWrapperType, parameters) as TranslatorWrapperBase;
        }
    }
}