using App.Domain.Comparers;
using App.Domain.NotMapped;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace DAL;

public class SqliteAppDbContext : AbstractAppDbContext
{
    public SqliteAppDbContext(DbContextOptions<SqliteAppDbContext> options, IConfiguration configuration) : base(options, configuration)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(List<string>))
                    property.SetValueComparer(new StringListValueComparer());
                SetJsonValueComparers<ImageFile>(property);
                SetJsonValueComparers<VideoFile>(property);
                SetJsonValueComparers<Caption>(property);
                SetJsonValueComparers<LangString>(property);
            }
        }
    }

    private static void SetJsonValueComparers<TProperty>(IMutableProperty? property) where TProperty : new()
    {
        if (property == null) return;
        if (property.ClrType == typeof(TProperty))
            property.SetValueComparer(new JsonSerializableValueComparer<TProperty>());
        if (property.ClrType == typeof(List<TProperty>))
            property.SetValueComparer(new JsonSerializableValueComparer<List<TProperty>>());
        if (property.ClrType == typeof(Dictionary<string, TProperty>))
            property.SetValueComparer(new JsonSerializableValueComparer<Dictionary<string, TProperty>>());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder
            .HaveJsonConversion<List<string>>()
            .HaveTextJsonConversions<ImageFile>()
            .HaveTextJsonConversions<VideoFile>()
            .HaveTextJsonConversions<Caption>()
            .HaveTextJsonConversions<LangString>();
    }
}