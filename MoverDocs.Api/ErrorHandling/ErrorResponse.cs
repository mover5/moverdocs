using System;
using Newtonsoft.Json;

namespace MoverDocs.Api.ErrorHandling
{
    public class ErrorResponse
    {
        [JsonProperty]
        public ErrorResponseMessage Error { get; set; }
    }

    public class ErrorResponseMessage
    {
        [JsonProperty]
        public string Message { get; set; }

        [JsonProperty]
        public ErrorResponseCode Code { get; set; }

        [JsonProperty]
        public Exception Exception { get; set; }
    }
}