using Newtonsoft.Json;

namespace MoverDocs.Api.Definitions
{
    public class ExternalDocumentation
    {
        [JsonProperty]
        public string Description { get; set; }

        [JsonProperty]
        public string Url { get; set; }
    }
}
