using System.Net.Http;
using System.Web.Http;

namespace MoverDocs.Api.Handlers
{
    public class BaseHandler : DelegatingHandler
    {
        public HttpConfiguration HttpConfiguration { get; set; }

        public BaseHandler(HttpConfiguration httpConfiguration)
        {
            this.HttpConfiguration = httpConfiguration;
        }
    }
}