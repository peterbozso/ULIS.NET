using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ulis.Net.Library.Luis
{
    public class LuisResult
    {
        [JsonProperty(PropertyName = "entities")]
        public IList<EntityRecommendation> Entities { get; set; }

        [JsonProperty(PropertyName = "intents")]
        public IList<IntentRecommendation> Intents { get; set; }

        [JsonProperty(PropertyName = "query")]
        public string Query { get; set; }

        [JsonProperty(PropertyName = "topScoringIntent")]
        public IntentRecommendation TopScoringIntent { get; set; }
    }
}