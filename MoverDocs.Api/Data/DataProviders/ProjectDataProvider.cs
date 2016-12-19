using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using MoverDocs.Api.Data.Tables;
using MoverSoft.StorageLibrary.Entities;
using MoverSoft.StorageLibrary.Tables;

namespace MoverDocs.Api.Data.DataProviders
{
    public class ProjectDataProvider
    {
        private TableStorageDataProvider TableStorageDataProject { get; set; }

        private const string TableName = "projects";

        public ProjectDataProvider(string connectionString)
        {
            this.TableStorageDataProject = new TableStorageDataProvider(
                connectionString: connectionString,
                tableName: ProjectDataProvider.TableName);
        }

        public Task SaveProject(Project project)
        {
            return this.TableStorageDataProject.SaveEntity(record: project);
        }

        public Task DeleteProject(Project project)
        {
            return this.TableStorageDataProject.DeleteEntity(record: project);
        }

        public Task<Project> FindProject(string organizationId, string projectId)
        {
            return this.TableStorageDataProject
                .Find<Project>(
                    partitionKey: Project.GetPartitionKey(organizationId),
                    rowKey: Project.IdIndex.GetRowKey(projectId));
        }

        public Task<SegmentedResult<Project>> FindProjects(string organizationId, TableContinuationToken continuationToken = null)
        {
            return this.TableStorageDataProject
                .FindRangeSegmented<Project>(
                    partitionKey: Project.GetPartitionKey(organizationId),
                    rowKeyPrefix: Project.IdIndex.GetRowKeyPrefix(),
                    token: continuationToken);
        }
    }
}