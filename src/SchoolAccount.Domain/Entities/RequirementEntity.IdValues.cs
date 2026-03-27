namespace SchoolAccount.Domain.Models.Entities;

public partial class RequirementEntity
{
    public static class IdValues
    {
        public const int Mandatory = 1;
        public const int Conditional = 2;
        public const int Optional = 3;
    }
}
