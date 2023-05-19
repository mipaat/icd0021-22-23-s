#pragma warning disable 1591
using System.ComponentModel.DataAnnotations;
using App.Common;

namespace WebApp.Validation;

public class LangStringNotEmptyAttribute : ValidationAttribute
{
    private readonly ICollection<string> _requiredCultures;

    public LangStringNotEmptyAttribute(params string[] requiredCultures)
    {
        _requiredCultures = requiredCultures;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not LangString langString) return new ValidationResult($"Not a {nameof(LangString)}");

        if (langString.Keys.Count == 0)
            return new ValidationResult("At least one value is required");

        if (_requiredCultures.Count > 0)
        {
            var missingCultures = _requiredCultures.Select(e => !langString.ContainsKey(e)).ToList();
            if (missingCultures.Count > 0)
            {
                return new ValidationResult("Missing entries for required languages: " +
                                            string.Join(", ", missingCultures));
            }
        }

        if (langString.Keys.Any(k => k.Trim().Length == 0))
        {
            return new ValidationResult("Empty language key is not allowed");
        }
        
        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        var emptyValues = langString.Where(kvp => (kvp.Value?.Trim() ?? "").Length == 0).ToList();
        if (emptyValues.Count > 0)
        {
            return new ValidationResult("Empty values for cultures: " +
                                        string.Join(", ", emptyValues.Select(kvp => kvp.Key)));
        }

        return ValidationResult.Success;
    }
}