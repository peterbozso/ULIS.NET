using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ulis.Net.Library.Luis
{
    public class EntityRecommendation
    {
        [JsonProperty(PropertyName = "endIndex")]
        public int? EndIndex { get; set; }

        [JsonProperty(PropertyName = "entity")]
        public string Entity { get; set; }

        [JsonProperty(PropertyName = "resolution", ItemConverterType = typeof(ResolutionConverter))]
        public IDictionary<string, object> Resolution { get; set; }

        [JsonProperty(PropertyName = "startIndex")]
        public int? StartIndex { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}