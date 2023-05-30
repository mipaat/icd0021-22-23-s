#pragma warning disable 1591
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace App.Common.Validation;

public class LangStringNotEmptyAttribute : ValidationAttribute
{
    private readonly ICollection<string> _requiredCultures;

    public LangStringNotEmptyAttribute(params string[] requiredCultures)
    {
        _requiredCultures = requiredCultures;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var localizer = validationContext
            .GetRequiredService<IStringLocalizer<App.Resources.App.Common.Validation.LangStringNotEmptyAttribute>>();

        if (value is not LangString langString)
            return new ValidationResult(
                localizer[nameof(Resources.App.Common.Validation.LangStringNotEmptyAttribute.NotALangString)]);

        if (langString.Keys.Count == 0)
            return new ValidationResult(localizer[
                nameof(Resources.App.Common.Validation.LangStringNotEmptyAttribute.AtLeastOneValueRequired)]);

        if (_requiredCultures.Count > 0)
        {
            var missingCultures = _requiredCultures.Select(e => !langString.ContainsKey(e)).ToList();
            if (missingCultures.Count > 0)
            {
                return new ValidationResult(string.Format(
                    localizer[
                        nameof(Resources.App.Common.Validation.LangStringNotEmptyAttribute
                            .MissingEntriesForRequiredLanguages)],
                    string.Join(", ", missingCultures)));
            }
        }

        if (langString.Keys.Any(k => k.Trim().Length == 0))
        {
            return new ValidationResult(
                localizer[nameof(Resources.App.Common.Validation.LangStringNotEmptyAttribute.EmptyLanguageKey)]);
        }

        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        var emptyValues = langString.Where(kvp => (kvp.Value?.Trim() ?? "").Length == 0).ToList();
        if (emptyValues.Count > 0)
        {
            return new ValidationResult(string.Format(
                localizer[nameof(Resources.App.Common.Validation.LangStringNotEmptyAttribute.EmptyValues)],
                string.Join(", ", emptyValues.Select(kvp => kvp.Key))));
        }

        return ValidationResult.Success;
    }
}