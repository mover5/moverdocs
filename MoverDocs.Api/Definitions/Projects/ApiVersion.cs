using Newtonsoft.Json;

namespace MoverDocs.Api.Definitions
{
    public class ApiVersionDefinition
    {
        [JsonProperty]
        public string Id { get; set; }

        [JsonProperty]
        public string ApiVersion { get; set; }

        [JsonProperty]
        public string ProjectId { get; set; }

        [JsonProperty]
        public ProjectDefinition Project { get; set; }

        [JsonProperty]
        public string Description { get; set; }

        [JsonProperty]
        public bool Deprecated { get; set; }
    }
}