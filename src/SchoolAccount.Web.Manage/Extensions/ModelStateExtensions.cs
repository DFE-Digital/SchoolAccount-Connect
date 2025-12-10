using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Manage.Models;

namespace SchoolAccount.Web.Manage.Extensions;

public static class ModelStateExtensions
{
    public static void AddFluentValidationErrors(this ModelStateDictionary modelState, ValidationError validationResult, string prefix = "")
    {
        foreach (var failure in validationResult.Errors)
        {
            var propertyName = string.IsNullOrWhiteSpace(prefix)
                ? failure.Property
                : $"{prefix}.{failure.Property}";
            
            modelState.AddModelError(
                propertyName ?? string.Empty,
                failure.Description);
        }
    }
    
    private static bool HasErrors(this ModelStateDictionary modelState, string key)
    {
        return modelState.TryGetValue(key, out var entry) && entry.Errors.Count > 0;
    }
    
    private static bool HasErrorsAndGet(this ModelStateDictionary modelState, string key, out ModelStateEntry? entry)
    {
        return modelState.TryGetValue(key, out entry) && entry.Errors.Count > 0;
    }

    private static string? GetFirstErrorMessage(this ModelStateDictionary modelState, string key)
    {
        return modelState.HasErrorsAndGet(key, out var entry)
            ? entry!.Errors.FirstOrDefault()?.ErrorMessage
            : null;
    }

    public static FieldMetadataModel GetFieldMetadata<TModel>(this ViewDataDictionary<TModel> viewData)
    {
        var modelState = viewData.ModelState;
        var metadata = viewData.ModelMetadata;
        var templateInfo = viewData.TemplateInfo;
        
        var fieldName = templateInfo.GetFullHtmlFieldName("");
        viewData.TryGetValue("hint", out var hintMessage);
        
        return new FieldMetadataModel
        {
            FieldName = templateInfo.GetFullHtmlFieldName(""),
            FieldId = TagBuilder.CreateSanitizedId(fieldName, "_"),
            Label = metadata.GetDisplayName(),
            Description = metadata.Description,
            Hint = hintMessage?.ToString(),
            IsRequired = metadata.IsRequired(), // metadata.IsRequired,
            HasError = modelState.HasErrors(fieldName),
            ErrorMessage = modelState.GetFirstErrorMessage(fieldName)
        };
    }
}