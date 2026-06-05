using SchoolAccount.Domain.Resources;

namespace SchoolAccount.Tests.Common.Builders.Tasks;

public sealed class ResourceBuilder
{
    private string _name = "Test Resource";
    private string _link = "https://test.com";

    public static ResourceBuilder AResource() => new();

    public ResourceBuilder Named(string name)
    {
        _name = name;
        return this;
    }

    public ResourceBuilder WithLink(string link)
    {
        _link = link;
        return this;
    }

    public ResourceEntity Build()
    {
        return new ResourceEntity { ResourceName = _name, DigitalLink = _link };
    }
}
