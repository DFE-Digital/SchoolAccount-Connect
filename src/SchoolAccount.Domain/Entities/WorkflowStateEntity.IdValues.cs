namespace SchoolAccount.Domain.Models.Entities;

public partial class WorkflowStateEntity
{
    public static class IdValues
    {
        public const int Draft = 1;
        public const int Queued = 2;
        public const int Published = 3;
        public const int Expired = 4;
        public const int Archived = 5;
    }
}
