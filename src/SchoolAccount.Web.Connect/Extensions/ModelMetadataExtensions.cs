using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SchoolAccount.Web.Connect.Extensions;

public static class ModelMetadataExtensions
{
    public static bool IsRequired(this ModelMetadata metadata)
    {
        var name = metadata.PropertyName;
        
        if (string.IsNullOrEmpty(name))
        {
            return false;
        }
        
        var propertyInfo = metadata.ContainerType?.GetProperty(name);
        return propertyInfo?
            .GetCustomAttributes(typeof(RequiredAttribute), inherit: true)
            .Length == 0;
    }
}