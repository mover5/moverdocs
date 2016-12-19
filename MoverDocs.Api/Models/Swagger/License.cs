using Newtonsoft.Json;

namespace MoverDocs.Api.Models
{
    public class License
    {
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Url { get; set; }
    }
}
