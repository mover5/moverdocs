using System;
using System.Collections.Generic;
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
    public class ProjectsController : BaseApiController
    {
        [HttpGet]
        public async Task<HttpResponseMessage> ListProjects()
        {
            var projects = await this
                .GetProjectDataProvider()
                .FindProjects(organizationId: Project.GlobalProject)
                .ConfigureAwait(continueOnCapturedContext: false);

            // TODO: Move the $expand logic into query string reader
            var queryStringReader = new QueryStringReader(requestUri: this.Request.RequestUri);
            var expandApiVersions = queryStringReader.QueryParameters.ContainsKey("$expand") && queryStringReader.QueryParameters["$expand"].EqualsInsensitively("apiVersions");

            IEnumerable<ApiVersion> apiVersions = null;
            if (expandApiVersions)
            {
                var allApiVersions = await this
                    .GetApiVersionDataProvider()
                    .FindApiVersions(organizationId: Project.GlobalProject)
                    .ConfigureAwait(continueOnCapturedContext: false);

                apiVersions = allApiVersions.Results.CoalesceEnumerable();
            }

            var projectDefinitions = projects.Results
                .CoalesceEnumerable()
                .Select(project =>
                {
                    ApiVersionDefinition[] projectApiVersions = null;
                    if (expandApiVersions)
                    {
                        projectApiVersions = apiVersions
                            .Where(apiVersion => apiVersion.ProjectId.EqualsInsensitively(project.ProjectId))
                            .SelectArray(apiVersion => apiVersion.ToDefinition());
                    }

                    return project.ToDefinition(apiVersions: projectApiVersions);
                });

            return this.Request.CreateResponse(
                statusCode: HttpStatusCode.OK,
                value: projectDefinitions,
                configuration: this.HttpConfiguration,
                encodedContinuationToken: null);
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetProject(string projectName)
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

            // TODO: Move the $expand logic into query string reader
            var queryStringReader = new QueryStringReader(requestUri: this.Request.RequestUri);
            var expandApiVersions = queryStringReader.QueryParameters.ContainsKey("$expand") && queryStringReader.QueryParameters["$expand"].EqualsInsensitively("apiVersions");

            ApiVersionDefinition[] apiVersionDefinitions = null;
            if (expandApiVersions)
            {
                var apiVersions = await this
                    .GetApiVersionDataProvider()
                    .FindApiVersions(
                        organizationId: Project.GlobalProject,
                        projectId: projectName)
                    .ConfigureAwait(continueOnCapturedContext: false);

                apiVersionDefinitions = apiVersions.Results
                    .CoalesceEnumerable()
                    .SelectArray(apiVersion => apiVersion.ToDefinition());
            }

            return this.Request.CreateResponse(
                statusCode: HttpStatusCode.OK,
                value: project.ToDefinition(apiVersionDefinitions),
                configuration: this.HttpConfiguration);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> CreateProject()
        {
            var projectDefinition = await this.Request.Content
                .ReadAsAsync<ProjectDefinition>()
                .ConfigureAwait(continueOnCapturedContext: false);

            if (projectDefinition == null)
            {
                throw new ErrorResponseMessageException(
                    httpStatus: HttpStatusCode.BadRequest,
                    errorCode: ErrorResponseCode.ProjectNull,
                    errorMessage: "The project request cannot be null");
            }

            if (string.IsNullOrEmpty(projectDefinition.DisplayName))
            {
                throw new ErrorResponseMessageException(
                    httpStatus: HttpStatusCode.BadRequest,
                    errorCode: ErrorResponseCode.ProjectMissingDisplayName,
                    errorMessage: "The project requires a display name");
            }

            if (string.IsNullOrEmpty(projectDefinition.Host))
            {
                throw new ErrorResponseMessageException(
                    httpStatus: HttpStatusCode.BadRequest,
                    errorCode: ErrorResponseCode.ProjectMissingHostUrl,
                    errorMessage: "The project must have a host url");
            }

            var existingProjects = await this
                .GetProjectDataProvider()
                .FindProjectsByDisplayName(
                    organizationId: Project.GlobalProject,
                    displayName: projectDefinition.DisplayName)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (existingProjects.Results.CoalesceEnumerable().Any())
            {
                throw new ErrorResponseMessageException(
                    httpStatus: HttpStatusCode.Conflict,
                    errorCode: ErrorResponseCode.ProjectDisplayNameAlreadyExists,
                    errorMessage: string.Format("A project with display name {0} already exists. Please chose a different name", projectDefinition.DisplayName));
            }

            var project = Project.CreateProjectFromDefinition(projectDefinition, Project.GlobalProject);

            await this
                .GetProjectDataProvider()
                .SaveProject(project)
                .ConfigureAwait(continueOnCapturedContext: false);

            return this.Request.CreateResponse(
                statusCode: HttpStatusCode.Created,
                value: project.ToDefinition(),
                configuration: this.HttpConfiguration);
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteProject(string projectName)
        {
            var project = await this
                .GetProjectDataProvider()
                .FindProject(
                    organizationId: Project.GlobalProject,
                    projectId: projectName)
                .ConfigureAwait(continueOnCapturedContext: false);

            if (project != null)
            {
                var apiVersions = await this
                    .GetApiVersionDataProvider()
                    .FindApiVersions(
                        organizationId: Project.GlobalProject,
                        projectId: projectName)
                    .ConfigureAwait(continueOnCapturedContext: false);

                if (apiVersions.Results.CoalesceEnumerable().Any())
                {
                    await this
                        .GetApiVersionDataProvider()
                        .DeleteApiVersions(apiVersions.Results)
                        .ConfigureAwait(continueOnCapturedContext: false);
                }

                await this
                    .GetProjectDataProvider()
                    .DeleteProject(project)
                    .ConfigureAwait(continueOnCapturedContext: false);

                return this.Request.CreateResponse(statusCode: HttpStatusCode.OK);
            }

            return this.Request.CreateResponse(statusCode: HttpStatusCode.NoContent);
        }
    }
}