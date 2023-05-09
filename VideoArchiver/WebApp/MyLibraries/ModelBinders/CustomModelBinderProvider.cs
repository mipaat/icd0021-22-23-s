#pragma warning disable 1591
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebApp.MyLibraries.ModelBinders;

public class CustomModelBinderProvider<T> : IModelBinderProvider
{
    private static Type Type => typeof(T);

    private static List<ICustomModelBinder> ModelBinders => new()
    {
        new PlatformModelBinder()
    };

    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        var modelType = context.Metadata.UnderlyingOrModelType;

        return modelType != Type ? null : ModelBinders.Find(mb => mb.Type == Type);
    }
}