using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Routing;
using MoverDocs.Api.Handlers;
using MoverSoft.Common.Extensions;

namespace MoverDocs.Api.Initialization
{
    public class HttpConfigurationInitialization
    {
        public static readonly HttpConfigurationInitialization Instance = new HttpConfigurationInitialization();

        public void Initialize(HttpConfiguration httpConfiguration)
        {
            this.ConfigureMediaTypeFormatters(httpConfiguration);
            this.RegisterWebApiRoutes(httpConfiguration);
            this.ConfigureMessageHandlers(httpConfiguration);
            this.ConfigureFilters(httpConfiguration);

            var corsAttr = new EnableCorsAttribute("*", "*", "*");
            httpConfiguration.EnableCors(corsAttr);
        }

        private void ConfigureMediaTypeFormatters(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.Formatters.Clear();
            httpConfiguration.Formatters.Add(JsonExtensions.JsonMediaTypeFormatter);
        }

        private void ConfigureMessageHandlers(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.MessageHandlers.Add(new ErrorResponseHandler(httpConfiguration));
        }

        private void ConfigureFilters(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.Filters.Add(new ErrorResponseFilter(httpConfiguration));
        }

        private void RegisterWebApiRoutes(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.Routes.MapHttpRoute(
                name: "ProjectCollectionRequests",
                routeTemplate: "api/projects",
                defaults: new { controller = "Projects" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get, HttpMethod.Post) });

            httpConfiguration.Routes.MapHttpRoute(
                name: "ProjectRequests",
                routeTemplate: "api/projects/{projectName}",
                defaults: new { controller = "Projects" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get, HttpMethod.Delete) });

            httpConfiguration.Routes.MapHttpRoute(
                name: "ApiVersionCollectionRequests",
                routeTemplate: "api/projects/{projectName}/apiVersions",
                defaults: new { controller = "ApiVersions" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });

            httpConfiguration.Routes.MapHttpRoute(
                name: "ApiVersionRequests",
                routeTemplate: "api/projects/{projectName}/apiVersions/{apiVersionValue}",
                defaults: new { controller = "ApiVersions" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get, HttpMethod.Put, HttpMethod.Delete) });
        }
    }
}