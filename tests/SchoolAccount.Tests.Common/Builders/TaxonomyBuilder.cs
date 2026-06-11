using SchoolAccount.Domain.Tags;
using SchoolAccount.Domain.Taxonomies;

namespace SchoolAccount.Tests.Common.Builders;

public sealed class TaxonomyBuilder
{
    private int? _id;

    private string? _name;
    private string? _taxonomyName;

    private string? _displayName;

    private bool _isMandatory;

    private bool _isMultiSelect;

    private string? _description;

    private int? _taxonomyGroupingId;
    private TaxonomyGroupingEntity? _taxonomyGrouping;

    private List<TagBuilder> _tags = [];

    public static TaxonomyBuilder ATaxonomy() => new();

    public TaxonomyBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public TaxonomyBuilder WithName(string name)
    {
        _name = name;
        _taxonomyName = name;
        return this;
    }

    public TaxonomyBuilder WithDisplayName(string name)
    {
        _displayName = name;
        return this;
    }

    public TaxonomyBuilder IsMandatory()
    {
        _isMandatory = true;
        return this;
    }

    public TaxonomyBuilder IsMultiSelect()
    {
        _isMultiSelect = true;
        return this;
    }

    public TaxonomyBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public TaxonomyBuilder WithGrouping(TaxonomyGroupingEntity grouping)
    {
        _taxonomyGrouping = grouping;
        _taxonomyGroupingId = grouping.Id;
        return this;
    }

    public TaxonomyBuilder WithTag(TagBuilder tag)
    {
        _tags.Add(tag);
        return this;
    }

    public TaxonomyBuilder WithTags(params TagBuilder[] tags)
    {
        _tags.AddRange(tags);
        return this;
    }

    public TaxonomyEntity Build()
    {
        if (!_id.HasValue)
        {
            throw new ArgumentNullException(nameof(_id));
        }

        ArgumentNullException.ThrowIfNull(_name);
        ArgumentNullException.ThrowIfNull(_taxonomyName);

        var taxonomy = new TaxonomyEntity
        {
            Id = _id.Value,
            Name = _name,
            TaxonomyName = _taxonomyName,
            DisplayName = _displayName,
            IsMandatory = _isMandatory,
            IsMultiSelect = _isMultiSelect,
            Description = _description,
            TaxonomyGroupingId = _taxonomyGroupingId,
            TaxonomyGrouping = _taxonomyGrouping,
        };

        foreach (var tag in _tags)
        {
            tag.WithTaxonomy(taxonomy, true);
            taxonomy.Tags.Add(tag.Build());
        }

        return taxonomy;
    }

    public static implicit operator TaxonomyEntity(TaxonomyBuilder builder)
    {
        return builder.Build();
    }
}
