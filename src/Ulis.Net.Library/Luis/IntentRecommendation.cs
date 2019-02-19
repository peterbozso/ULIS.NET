using Newtonsoft.Json;

namespace Ulis.Net.Library.Luis
{
    public class IntentRecommendation
    {
        [JsonProperty(PropertyName = "intent")]
        public string Intent { get; set; }

        [JsonProperty(PropertyName = "score")]
        public double? Score { get; set; }
    }
}