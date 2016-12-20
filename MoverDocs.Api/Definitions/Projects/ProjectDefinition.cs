using Newtonsoft.Json;

namespace MoverDocs.Api.Definitions
{
    public class ProjectDefinition
    {
        [JsonProperty]
        public string Id { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string DisplayName { get; set; }

        [JsonProperty]
        public string ContactEmail { get; set; }

        [JsonProperty]
        public string Description { get; set; }

        [JsonProperty]
        public string Host { get; set; }

        [JsonProperty]
        public string[] Schemes { get; set; }

        [JsonProperty]
        public string BasePath { get; set; }

        [JsonProperty]
        public ApiVersionDefinition[] ApiVersions { get; set; }
    }
}