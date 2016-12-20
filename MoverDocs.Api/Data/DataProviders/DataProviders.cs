using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MoverSoft.Common.Configuration;

namespace MoverDocs.Api.Data.DataProviders
{
    public class DataProviders
    {
        public static Lazy<DataProviders> Instance = new Lazy<DataProviders>(valueFactory: () => new DataProviders());

        private string ConnectionString
        {
            get
            {
                return ConfigurationManager.GetConnectionString(
                    keyName: "DataStorageConnectionString",
                    defaultValue: "UseDevelopmentStorage=true;");
            }
        }

        public ProjectDataProvider ProjectDataProvider { get; set; }

        public ApiVersionDataProvider ApiVersionDataProvider { get; set; }

        public DataProviders()
        {
            this.ProjectDataProvider = new ProjectDataProvider(this.ConnectionString);

            this.ApiVersionDataProvider = new ApiVersionDataProvider(this.ConnectionString);
        }
    }
}