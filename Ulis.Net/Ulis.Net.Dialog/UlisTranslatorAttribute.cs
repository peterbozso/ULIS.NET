using System;

namespace Ulis.Net.Dialog
{
    [AttributeUsage(AttributeTargets.Class)]
    [Serializable]
    public class UlisTranslatorAttribute : Attribute
    {
        //public TranslationProvider TranslationProvider { get; }
        //public string SubscriptionKey { get; }

        //public UlisTranslatorAttribute(TranslationProvider translationProvider, string subscriptionKey)
        //{
        //    TranslationProvider = translationProvider;
        //    SubscriptionKey = subscriptionKey;
        //}
    }
}