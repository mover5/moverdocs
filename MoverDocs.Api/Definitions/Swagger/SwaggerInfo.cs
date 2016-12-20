using Newtonsoft.Json;

namespace MoverDocs.Api.Definitions
{
    public class SwaggerInfo
    {
        [JsonProperty]
        public string Title { get; set; }

        [JsonProperty]
        public string Description { get; set; }

        [JsonProperty]
        public string TermsOfService { get; set; }

        [JsonProperty]
        public Contact Contact { get; set; }

        [JsonProperty]
        public License License { get; set; }

        [JsonProperty]
        public string Version { get; set; }
    }
}
