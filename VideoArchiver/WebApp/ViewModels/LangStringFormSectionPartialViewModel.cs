#pragma warning disable CS1591
namespace WebApp.ViewModels;

public class LangStringFormSectionPartialViewModel
{
    public LangStringFormSectionPartialViewModel(string paramName, string culture, string? value, bool disabled = false)
    {
        ParamName = paramName;
        Culture = culture;
        Value = value ?? "";
        Disabled = disabled;
    }

    public string ParamName { get; init; }
    public string Culture { get; init; }
    public string Value { get; init; }
    public bool Disabled { get; set; }
}