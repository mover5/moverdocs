using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MoverDocs.Api.Configuration;
using MoverDocs.Api.Data.Tables;
using MoverDocs.Api.Definitions;
using MoverDocs.Api.ErrorHandling;
using MoverSoft.Common.Extensions;
using MoverSoft.Common.Utilities;
using MoverSoft.Common.Web;

namespace MoverDocs.Api.Controllers
{
    public class ApiVersionsController : BaseApiController
    {
        [HttpGet]
        public async Task<HttpResponseMessage> ListProjectApiVersions(string projectName)
        {
            var project = await this
                .GetProjectDataProvider()
                .FindProject(
                    organizationId: Project.GlobalProject,
                    projectId: projectName)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (project == null)
            {
                throw new ErrorResponseMessageException(
                    httpStatus: HttpStatusCode.NotFound,
                    errorCode: ErrorResponseCode.ProjectNotFound,
                    errorMessage: string.Format("The project with name {0} was not found", projectName));
            }

            var apiVersions = await this
                .GetApiVersionDataProvider()
                .FindApiVersions(
                    organizationId: Project.GlobalProject,
                    projectId: projectName)
                .ConfigureAwait(continueOnCapturedContext: false);

            // TODO: Move the $expand logic into query string reader
            var queryStringReader = new QueryStringReader(requestUri: this.Request.RequestUri);
            var expandProject = queryStringReader.QueryParameters.ContainsKey("$expand") && queryStringReader.QueryParameters["$expand"].EqualsInsensitively("project");

            return this.Request.CreateResponse(
                statusCode: HttpStatusCode.OK,
                value: apiVersions.Results.CoalesceEnumerable().Select(apiVersion => apiVersion.ToDefinition(expandProject ? project : null)),
                configuration: this.HttpConfiguration,
                encodedContinuationToken: null);
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetProjectApiVersion(string projectName, string apiVersionValue)
        {
            var project = await this
                .GetProjectDataProvider()
                .FindProject(
                    organizationId: Project.GlobalProject,
                    projectId: projectName)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (project == null)
            {
                throw new ErrorResponseMessageException(
                    httpStatus: HttpStatusCode.NotFound,
                    errorCode: ErrorResponseCode.ProjectNotFound,
                    errorMessage: string.Format("The project with name {0} was not found", projectName));
            }

            var apiVersion = await this
                .GetApiVersionDataProvider()
                .FindApiVersion(
                    organizationId: Project.GlobalProject,
                    projectId: projectName,
                    apiVersionValue: apiVersionValue)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (apiVersion == null)
            {
                throw new ErrorResponseMessageException(
                    httpStatus: HttpStatusCode.NotFound,
                    errorCode: ErrorResponseCode.ApiVersionNotFound,
                    errorMessage: string.Format("The api version {0} in project {1} was not found", apiVersionValue, projectName));
            }

            // TODO: Move the $expand logic into query string reader
            var queryStringReader = new QueryStringReader(requestUri: this.Request.RequestUri);
            var expandProject = queryStringReader.QueryParameters.ContainsKey("$expand") && queryStringReader.QueryParameters["$expand"].EqualsInsensitively("project");

            return this.Request.CreateResponse(
                statusCode: HttpStatusCode.OK,
                value: apiVersion.ToDefinition(expandProject ? project : null),
                configuration: this.HttpConfiguration);
        }

        [HttpPut]
        public async Task<HttpResponseMessage> CreateApiVersion(string projectName, string apiVersionValue)
        {
            var project = await this
                .GetProjectDataProvider()
                .FindProject(
                    organizationId: Project.GlobalProject,
                    projectId: projectName)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (project == null)
            {
                throw new ErrorResponseMessageException(
                    httpStatus: HttpStatusCode.NotFound,
                    errorCode: ErrorResponseCode.ProjectNotFound,
                    errorMessage: string.Format("The project with name {0} was not found", projectName));
            }

            var apiVersionDefinition = await this.Request.Content
                .ReadAsAsync<ApiVersionDefinition>()
                .ConfigureAwait(continueOnCapturedContext: false);

            if (apiVersionDefinition == null)
            {
                throw new ErrorResponseMessageException(
                    httpStatus: HttpStatusCode.BadRequest,
                    errorCode: ErrorResponseCode.ApiVersionNull,
                    errorMessage: "The api version request cannot be null");
            }

            if (string.IsNullOrEmpty(apiVersionValue)) // TODO Validate string contents for valid url characters
            {
                throw new ErrorResponseMessageException(
                    httpStatus: HttpStatusCode.BadRequest,
                    errorCode: ErrorResponseCode.ApiVersionInvalid,
                    errorMessage: string.Format("The requested api version '{0}' is invalid.", apiVersionValue));
            }

            apiVersionDefinition.ApiVersion = apiVersionValue;

            var apiVersion = ApiVersion.CreateApiVersionFromDefinition(
                definition: apiVersionDefinition,
                organizationId: Project.GlobalProject,
                projectName: projectName);

            await this
                .GetApiVersionDataProvider()
                .SaveApiVersion(apiVersion)
                .ConfigureAwait(continueOnCapturedContext: false);

            // TODO: Copy most recent api version's swagger definition

            return this.Request.CreateResponse(
                statusCode: HttpStatusCode.Created,
                value: apiVersion.ToDefinition(),
                configuration: this.HttpConfiguration);
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteApiVersion(string projectName, string apiVersionValue)
        {
            var apiVersion = await this
                .GetApiVersionDataProvider()
                .FindApiVersion(
                    organizationId: Project.GlobalProject,
                    projectId: projectName,
                    apiVersionValue: apiVersionValue)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (apiVersion != null)
            {
                await this
                    .GetApiVersionDataProvider()
                    .DeleteApiVersion(apiVersion)
                    .ConfigureAwait(continueOnCapturedContext: false);

                return this.Request.CreateResponse(HttpStatusCode.OK);
            }

            return this.Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}