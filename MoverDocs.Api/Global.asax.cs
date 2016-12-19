using System.Web.Http;
using MoverDocs.Api.Initialization;

namespace MoverDocs.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            HttpConfigurationInitialization.Instance.Initialize(httpConfiguration: GlobalConfiguration.Configuration);
        }
    }
}
