using Newtonsoft.Json;

namespace MoverDocs.Api.Models
{
    public class Tag
    {
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Description { get; set; }
    }
}
