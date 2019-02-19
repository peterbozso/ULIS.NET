using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ulis.Net.Library.Luis
{
    public class LuisResult
    {
        [JsonProperty(PropertyName = "query")]
        public string Query { get; set; }

        [JsonProperty(PropertyName = "topScoringIntent")]
        public IntentRecommendation TopScoringIntent { get; set; }

        [JsonProperty(PropertyName = "intents")]
        public IList<IntentRecommendation> Intents { get; set; }

        [JsonProperty(PropertyName = "entities")]
        public IList<EntityRecommendation> Entities { get; set; }
    }
}