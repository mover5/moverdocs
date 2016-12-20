using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using MoverDocs.Api.Data.Tables;
using MoverSoft.Common.Extensions;
using MoverSoft.StorageLibrary.Entities;
using MoverSoft.StorageLibrary.Tables;

namespace MoverDocs.Api.Data.DataProviders
{
    public class ApiVersionDataProvider
    {
        private TableStorageDataProvider TableStorageDataProject { get; set; }

        private const string TableName = "apiversions";

        public ApiVersionDataProvider(string connectionString)
        {
            this.TableStorageDataProject = new TableStorageDataProvider(
                connectionString: connectionString,
                tableName: ApiVersionDataProvider.TableName);
        }

        public Task SaveApiVersion(ApiVersion apiVersion)
        {
            return this.TableStorageDataProject.SaveEntity(record: apiVersion);
        }

        public Task DeleteApiVersion(ApiVersion apiVersion)
        {
            return this.TableStorageDataProject.DeleteEntity(record: apiVersion);
        }

        public Task DeleteApiVersions(IEnumerable<ApiVersion> apiVersions)
        {
            return this.TableStorageDataProject.DeleteEntities(apiVersions);
        }

        public Task<SegmentedResult<ApiVersion>> FindApiVersions(string organizationId, TableContinuationToken token = null)
        {
            return this.TableStorageDataProject
                .FindRangeSegmented<ApiVersion>(
                    partitionKey: ApiVersion.GetPartitionKey(organizationId),
                    rowKeyPrefix: ApiVersion.ApiVersionIndex.GetRowKeyPrefix(),
                    token: token);
        }

        public Task<SegmentedResult<ApiVersion>> FindApiVersions(string organizationId, string projectId, TableContinuationToken token = null)
        {
            return this.TableStorageDataProject
                .FindRangeSegmented<ApiVersion>(
                    partitionKey: ApiVersion.GetPartitionKey(organizationId),
                    rowKeyPrefix: ApiVersion.ApiVersionIndex.GetRowKeyPrefix(projectId),
                    token: token);
        }

        public Task<ApiVersion> FindApiVersion(string organizationId, string projectId, string apiVersionValue)
        {
            return this.TableStorageDataProject
                .Find<ApiVersion>(
                    partitionKey: ApiVersion.GetPartitionKey(organizationId),
                    rowKey: ApiVersion.ApiVersionIndex.GetRowKey(projectId, apiVersionValue));
        }
    }
}