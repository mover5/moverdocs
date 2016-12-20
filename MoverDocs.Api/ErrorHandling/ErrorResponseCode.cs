using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoverDocs.Api.ErrorHandling
{
    public enum ErrorResponseCode
    {
        NotSpecified,

        InternalServerError,

        NotFound,

        InvalidQueryString,

        ProjectNotFound,

        ProjectNull,

        ProjectMissingDisplayName,

        ProjectMissingHostUrl,

        ProjectDisplayNameAlreadyExists,

        ApiVersionNotFound,

        ApiVersionNull,

        ApiVersionInvalid,
    }
}