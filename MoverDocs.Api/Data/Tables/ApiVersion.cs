using System;
using MoverDocs.Api.Definitions;
using MoverDocs.Api.Engines;
using MoverSoft.StorageLibrary.Entities;
using MoverSoft.StorageLibrary.Tables;

namespace MoverDocs.Api.Data.Tables
{
    public class ApiVersion : TableRecord
    {
        public ApiVersion() : base() { }

        public ApiVersion(ApiVersion source) : base(source) { }

        [TableColumn]
        public string OrganizationId { get; set; }

        [TableColumn]
        public string ProjectId { get; set; }

        [TableColumn]
        public string ApiVersionValue { get; set; }

        [TableColumn]
        public string Description { get; set; }

        [TableColumn]
        public bool Deprecated { get; set; }

        public override string PartitionKey
        {
            get
            {
                return ApiVersion.GetPartitionKey(this.OrganizationId);
            }
        }

        public override string RowKey
        {
            get { throw new Exception("Use index"); }
        }

        public override TableRecord[] Indexes
        {
            get
            {
                return new TableRecord[]
                {
                    new ApiVersionIndex(this),
                };
            }
        }

        public static ApiVersion CreateApiVersionFromDefinition(ApiVersionDefinition definition, string organizationId, string projectName)
        {
            return new ApiVersion
            {
                ApiVersionValue = definition.ApiVersion,
                Description = definition.Description,
                Deprecated = definition.Deprecated,
                ProjectId = projectName,
                OrganizationId = organizationId
            };
        }

        public ApiVersionDefinition ToDefinition(Project expandedProject = null)
        {
            return new ApiVersionDefinition
            {
                Id = IdEngine.GetApiVersionId(this.ProjectId, this.ApiVersionValue),
                ProjectId = IdEngine.GetProjectId(this.ProjectId),
                Project = expandedProject != null ? expandedProject.ToDefinition() : null,
                ApiVersion = this.ApiVersionValue,
                Deprecated = this.Deprecated,
                Description = this.Description
            };
        }

        public static string GetPartitionKey(string orgId)
        {
            return orgId;
        }

        public class ApiVersionIndex : ApiVersion
        {
            private const string IndexId = "AVI";

            public ApiVersionIndex() : base() { }

            public ApiVersionIndex(ApiVersion source) : base(source) { }

            public override string RowKey
            {
                get { return ApiVersionIndex.GetRowKey(this.ProjectId, this.ApiVersionValue); }
            }

            public static string GetRowKey(string projectId, string apiVersionValue)
            {
                return TableStorageUtilities.CombineStorageKeys(
                    ApiVersionIndex.IndexId,
                    TableStorageUtilities.EscapeStorageKey(projectId.ToUpperInvariant()),
                    TableStorageUtilities.EscapeStorageKey(apiVersionValue.ToUpperInvariant()));
            }

            public static string GetRowKeyPrefix(string projectId)
            {
                return TableStorageUtilities.CombineStorageKeys(
                    ApiVersionIndex.IndexId,
                    TableStorageUtilities.EscapeStorageKey(projectId.ToUpperInvariant()));
            }

            public static string GetRowKeyPrefix()
            {
                return TableStorageUtilities.CombineStorageKeys(
                    ApiVersionIndex.IndexId);
            }
        }
    }
}