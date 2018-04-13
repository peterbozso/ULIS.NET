﻿using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Ulis.Net.Library;

namespace Ulis.Net.Dialog
{
    /// <summary>
    /// Needed because of this: https://github.com/dotnet/standard/issues/300
    /// Essentially our ITranslatorClient implementations cannot be made Serializable, so we wrap them this way.
    /// </summary>
    [Serializable]
    public abstract class TranslatorWrapperBase : ITranslatorClient
    {
        [NonSerialized]
        protected ITranslatorClient _translationClient;

        protected readonly string _subscriptionKey;

        protected TranslatorWrapperBase(string subscriptionKey)
        {
            _subscriptionKey = subscriptionKey;
        }

        public async Task<string> TranslateAsync(string text)
        {
            return await _translationClient.TranslateAsync(text);
        }

        [OnDeserialized]
        private void SetValuesOnDeserialized(StreamingContext context)
        {
            SetTranslatorClient();
        }

        // Must be called in each subclass's constructor!
        // The reason why we don't call it in this class's constructor: https://bit.ly/2Hm8XBy
        protected abstract void SetTranslatorClient();
    }
}