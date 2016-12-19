using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoverDocs.Api.Configuration
{
    public class BaseConfiguration
    {
        public static Lazy<BaseConfiguration> Instance = new Lazy<BaseConfiguration>(valueFactory: () => new BaseConfiguration());
    }
}