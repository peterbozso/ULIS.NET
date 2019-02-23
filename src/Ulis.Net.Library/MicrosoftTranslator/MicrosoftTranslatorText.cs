using Newtonsoft.Json;

namespace Ulis.Net.Library.MicrosoftTranslator
{
    internal class MicrosoftTranslatorText
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }
}