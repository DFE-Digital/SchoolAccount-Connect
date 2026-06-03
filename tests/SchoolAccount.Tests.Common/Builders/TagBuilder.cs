using SchoolAccount.Domain.SchoolTypes;
using SchoolAccount.Domain.Tags;
using SchoolAccount.Domain.Taxonomies;
using SchoolAccount.Kernel;

namespace SchoolAccount.Tests.Common.Builders;

public sealed class TagBuilder
{
    private long? _id;

    private int? _taxonomyId;
    private TaxonomyEntity? _taxonomy;

    private string? _tagName;
    private string? _name;

    private string? _displayName;

    private string? _description;

    private long? _parentTagId;
    
    public static TagBuilder ATag() => new();

    public TagBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public TagBuilder WithTaxonomy(TaxonomyEntity taxonomy, bool skipIfPopulated = false)
    {
        if (skipIfPopulated && _taxonomy is not null)
        {
            return this;
        }

        _taxonomyId = taxonomy.Id;
        _taxonomy = taxonomy;
        return this;
    }

    public TagBuilder WithName(string name)
    {
        _tagName = $"#{name}";
        _name = name;
        return this;
    }

    public TagBuilder WithDisplayName(string displayName)
    {
        _displayName = displayName;
        return this;
    }

    public TagBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public TagBuilder WithParent(long parentTagId)
    {
        _parentTagId = parentTagId;
        return this;
    }

    public TagBuilder WithParent(TagEntity tag)
    {
        _parentTagId = tag.Id;
        return this;
    }

    public TagEntity Build()
    {
        if (!_id.HasValue)
        {
            throw new ArgumentNullException(nameof(_id));
        }

        ArgumentNullException.ThrowIfNull(_name);
        ArgumentNullException.ThrowIfNull(_tagName);
        
        if (!_taxonomyId.HasValue)
        {
            throw new ArgumentNullException(nameof(_taxonomyId));
        }
        ArgumentNullException.ThrowIfNull(_taxonomy);

        return new TagEntity
        {
            Id = _id.Value,
            Name = _name,
            TagName =  _tagName,
            DisplayName = _displayName,
            Description = _description,
            ParentTagId = _parentTagId,
            Taxonomy = _taxonomy,
            TaxonomyId = _taxonomyId.Value
        };
    }

    public static SchoolTypeTagMappingEntity WithSchoolType(TagEntity tag, int id, SchoolType type)
    {
        return new SchoolTypeTagMappingEntity
        {
            Id = id,
            Tag = tag,
            TagId = tag.Id,
            SchoolType = new SchoolTypeEntity { Id = (int)type, Name = type.ToString() },
            SchoolTypeId = (int)type
        };
    }
    
    public static implicit operator TagEntity(TagBuilder builder)
    {
        return builder.Build();
    }
}