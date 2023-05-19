#pragma warning disable CS1591
namespace WebApp.ViewModels;

public class LangStringFormSectionPartialViewModel
{
    public LangStringFormSectionPartialViewModel(string paramName, string culture, string? value)
    {
        ParamName = paramName;
        Culture = culture;
        Value = value ?? "";
    }

    public string ParamName { get; init; }
    public string Culture { get; init; }
    public string Value { get; init; }
}