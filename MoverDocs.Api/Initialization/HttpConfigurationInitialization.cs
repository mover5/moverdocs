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
                name: "ListProjects",
                routeTemplate: "api/projects",
                defaults: new { controller = "Projects" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });
        }
    }
}