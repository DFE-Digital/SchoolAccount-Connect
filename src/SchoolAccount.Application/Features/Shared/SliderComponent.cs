using System.Collections.ObjectModel;
using SchoolAccount.Application.Extensions;

namespace SchoolAccount.Application.Features.Shared;

public class SliderComponent
{
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public NodeComponent Status { get; init; } = null!;
    public Collection<NodeComponent> Conditions { get; init; } = [];
    public Collection<NodeComponent> Metadata { get; init; } = [];
    public NodeComponent? Action { get; set; }
    public DateOnly? Due { get; init; }
}

public class NodeComponent
{
    public string Value { get; init; } = null!;
    public string? Group { get; init; }
    public string? Colour { get; init; }
    public string? Url { get; init; }
}

public class SliderCollection(IEnumerable<SliderComponent> slides)
    : Collection<SliderComponent>(slides.ToList())
{
    public SliderCollection() : this([])
    { }
};