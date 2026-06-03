using SchoolAccount.Application.Features.Shared.Query.Interfaces;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Shared.Query.Models;

public class QueryRow : Node<long>, IQueryRow
{
    public string? Description { get; init; }
    public DateOnly? SortDate { get; init; }
    public Node<int> Type { get; init; } = 0;
    public DateTime? LastUpdated { get; init; }
    public ExtensionNode<int>? Status { get; init; }
    public IEnumerable<ExtensionNode<long>> Types { get; init; } = [];
    public IEnumerable<ExtensionNode<long>> Tags { get; init; } = [];
}
