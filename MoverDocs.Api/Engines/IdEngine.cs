using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoverDocs.Api.Engines
{
    public static class IdEngine
    {
        public const string ProjectIdFormat = "/api/projects/{0}";

        public const string ApiVersionIdFormat = "/api/projects/{0}/apiVersions/{1}";

        public static string GetProjectId(string projectName)
        {
            return string.Format(IdEngine.ProjectIdFormat, projectName);
        }

        public static string GetApiVersionId(string projectName, string apiVersionValue)
        {
            return string.Format(IdEngine.ApiVersionIdFormat, projectName, apiVersionValue);
        }
    }
}