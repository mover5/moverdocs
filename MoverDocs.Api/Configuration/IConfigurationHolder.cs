using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MoverDocs.Api.Data.DataProviders;

namespace MoverDocs.Api.Configuration
{
    public interface IConfigurationHolder
    {
        DataProviders DataProviders { get; }

        BaseConfiguration BaseConfiguration { get; }
    }

    public static class IConfigurationHolderExtensions
    {
        public static ProjectDataProvider GetProjectDataProvider(this IConfigurationHolder holder)
        {
            return holder.DataProviders.ProjectDataProvider;
        }
    }
}