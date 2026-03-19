namespace SchoolAccount.Infrastructure.Configuration.Constants;

public static class TableConstants
{
    internal static class Administrative
    {
        public const string AuditLog = "AuditLog";
        public const string ProcessLog = "ProcessLog";
    }

    internal static class Transactional
    {
        public const string DelegatedContact = "DelegatedContact";
        public const string Resource = "Resource";
        public const string Services = "Services";
        public const string SubTask = "SubTask";
        public const string Task = "Task";
        public const string TaskDateSet = "TaskDateSet";
        public const string TaskRelation = "TaskRelation";
        public const string Team = "Team";
    }

    internal static class Mapping
    {
        public const string Resource = "ResourceSourceMapping";
        public const string Tag = "TagsSourceMapping";
        public const string Type = "TypeTaskMapping";
        public const string SchoolType = "SchoolTypeTagMapping";
        public const string Taxonomy = "TaxonomySourceAssociation";
    }

    internal static class Reference
    {
        public const string Directorate = "Directorate";
        public const string Group = "Group";
        public const string GuidanceType = "GuidanceType";
        public const string Requirement = "Requirement";
        public const string ResourceStatus = "ResourceStatus";
        public const string ResourceType = "ResourceType";
        public const string SchoolType = "SchoolType";
        public const string ServiceStatus = "ServiceStatus";
        public const string Source = "Source";
        public const string SupportLevel = "SupportLevel";
        public const string Tag = "Tag";
        public const string Taxonomy = "Taxonomy";
        public const string TaxonomyGrouping = "TaxonomyGrouping";
        public const string TeamStatus = "TeamStatus";
        public const string Type = "Type";
        public const string TypeGrouping = "TypeGrouping";
        public const string WorkflowState = "WorkflowState";
    }
}
