using System;
using MoverSoft.StorageLibrary.Entities;
using MoverSoft.StorageLibrary.Tables;

namespace MoverDocs.Api.Data.Tables
{
    public class Project : TableRecord
    {
        public Project() : base() { }

        public Project(Project source) : base(source) { }

        [TableColumn]
        public string OrganizationId { get; set; }

        [TableColumn]
        public string ProjectId { get; set; }

        [TableColumn]
        public string DisplayName { get; set; }

        [TableColumn]
        public string Description { get; set; }

        [TableColumn]
        public string ContactEmail { get; set; }

        public override string PartitionKey
        {
            get
            {
                return Project.GetPartitionKey(this.OrganizationId);
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
                    new NameIndex(this),
                    new IdIndex(this)
                };
            }
        }

        public static string GetPartitionKey(string orgId)
        {
            return orgId;
        }

        public class NameIndex : Project
        {
            private const string IndexId = "NAI";

            public NameIndex() : base() { }

            public NameIndex(Project source) : base(source) { }

            public override string RowKey
            {
                get { return NameIndex.GetRowKey(this.DisplayName, this.ProjectId); }
            }

            public static string GetRowKey(string displayName, string id)
            {
                return TableStorageUtilities.CombineStorageKeys(
                    NameIndex.IndexId,
                    TableStorageUtilities.EscapeStorageKey(displayName.ToUpper()),
                    TableStorageUtilities.EscapeStorageKey(id.ToUpper()));
            }

            public static string GetRowKeyPrefix(string displayName)
            {
                return TableStorageUtilities.CombineStorageKeys(
                    NameIndex.IndexId,
                    TableStorageUtilities.EscapeStorageKey(displayName.ToUpper()),
                    string.Empty);
            }

            public static string GetRowKeyPrefix()
            {
                return TableStorageUtilities.CombineStorageKeys(
                    NameIndex.IndexId,
                    string.Empty);
            }
        }

        public class IdIndex : Project
        {
            private const string IndexId = "IDI";

            public IdIndex() : base() { }

            public IdIndex(Project source) : base(source) { }

            public override string RowKey
            {
                get { return IdIndex.GetRowKey(this.ProjectId); }
            }

            public static string GetRowKey(string id)
            {
                return TableStorageUtilities.CombineStorageKeys(
                    IdIndex.IndexId,
                    TableStorageUtilities.EscapeStorageKey(id.ToUpper()));
            }

            public static string GetRowKeyPrefix()
            {
                return TableStorageUtilities.CombineStorageKeys(
                    IdIndex.IndexId,
                    string.Empty);
            }
        }
    }
}