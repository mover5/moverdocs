using System.Web.Http;
using MoverDocs.Api.Configuration;
using MoverDocs.Api.Data.DataProviders;

namespace MoverDocs.Api.Controllers
{
    public class BaseApiController : ApiController, IConfigurationHolder
    {
        public HttpConfiguration HttpConfiguration
        {
            get
            {
                return GlobalConfiguration.Configuration;
            }
        }

        public DataProviders DataProviders
        {
            get
            {
                return DataProviders.Instance.Value;
            }
        }

        public BaseConfiguration BaseConfiguration
        {
            get
            {
                return BaseConfiguration.Instance.Value;
            }
        }
    }
}