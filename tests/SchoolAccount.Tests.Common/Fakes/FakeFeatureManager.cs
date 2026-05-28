using System.Reflection;
using Microsoft.FeatureManagement;
using SchoolAccount.Application.Constants;

namespace SchoolAccount.Tests.Common.Fakes;

public class FakeFeatureManager : IFeatureManager
{
    private static readonly string[] FeatureNames = typeof(FeatureFlagConstants)
        .GetProperties(BindingFlags.Public | BindingFlags.Static)
        .Select(property => property.Name)
        .ToArray();

    private readonly HashSet<string> _enabledFeatures = new(StringComparer.Ordinal);

    public void Set(string feature, bool enabled)
    {
        if (enabled)
        {
            _enabledFeatures.Add(feature);
            return;
        }

        _enabledFeatures.Remove(feature);
    }

    public async IAsyncEnumerable<string> GetFeatureNamesAsync()
    {
        foreach (var featureName in FeatureNames)
        {
            yield return await Task.FromResult(featureName);
        }
    }

    public Task<bool> IsEnabledAsync(string feature)
    {
        return Task.FromResult(_enabledFeatures.Contains(feature));
    }

    public Task<bool> IsEnabledAsync<TContext>(string feature, TContext context)
    {
        return IsEnabledAsync(feature);
    }
}
