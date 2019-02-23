using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ulis.Net.Library.MicrosoftTranslator
{
    public partial class MicrosoftTranslatorClient
    {
        private class MicrosoftTranslatorResult
        {
            [JsonProperty(PropertyName = "translations")]
            public List<MicrosoftTranslatorText> Translations { get; set; }
        }
    }
}