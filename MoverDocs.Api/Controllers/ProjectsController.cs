using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MoverDocs.Api.Configuration;
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
                .FindProjects(organizationId: "global")
                .ConfigureAwait(continueOnCapturedContext: false);

            return this.Request.CreateResponse(
                statusCode: HttpStatusCode.OK,
                value: projects.Results,
                configuration: this.HttpConfiguration,
                encodedContinuationToken: null);
        }
    }
}