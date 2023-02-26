using Domain.Enums;

namespace WebApp.MyLibraries.ModelBinders;

public class PlatformModelBinder : CustomModelBinder<Platform>
{
    protected override Platform DefaultValue => Platform.This;

    protected override Func<string?, Platform?> Construct => s =>
    {
        if (s == null) return null;
        return s;
    };
}