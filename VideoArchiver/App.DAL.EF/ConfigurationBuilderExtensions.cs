using System.Linq.Expressions;
using App.Domain.Comparers;
using App.Domain.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DAL;

public static class ConfigurationBuilderExtensions
{
    public const string JsonB = "jsonb";
    public const string Text = "TEXT";

    public static ModelConfigurationBuilder HaveJsonConversion<TValue>(
        this ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<TValue>()
            .HaveConversion<JsonValueConverter<TValue>>();
        return configurationBuilder;
    }

    public static ModelConfigurationBuilder HaveJsonBColumnType<TValue>(
        this ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<TValue>()
            .HaveColumnType(JsonB);
        return configurationBuilder;
    }

    public static ModelConfigurationBuilder HaveJsonBConversions<TValue>(
        this ModelConfigurationBuilder configurationBuilder)
    {
        return configurationBuilder
            .HaveJsonConversion<TValue>()
            .HaveJsonBColumnTypes<TValue>();
    }

    public static ModelConfigurationBuilder HaveJsonBColumnTypes<TValue>(
        this ModelConfigurationBuilder modelConfigurationBuilder)
    {
        return modelConfigurationBuilder
            .HaveJsonBColumnType<TValue>()
            .HaveJsonBColumnType<List<TValue>>()
            .HaveJsonBColumnType<Dictionary<string, TValue>>();
    }

    public static ModelBuilder SetValueComparer<TEntity, TProperty>(
        this ModelBuilder modelBuilder,
        Expression<Func<TEntity, TProperty>> propertyExpression,
        ValueComparer<TProperty>? valueComparer = null)
        where TEntity : class where TProperty : new()
    {
        valueComparer ??= new JsonSerializableValueComparer<TProperty>();
        modelBuilder
            .Entity<TEntity>()
            .Property(propertyExpression)
            .Metadata
            .SetValueComparer(valueComparer);

        return modelBuilder;
    }

    public static ModelConfigurationBuilder HaveTextColumnType<TValue>(
        this ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<TValue>()
            .HaveColumnType(Text);
        return configurationBuilder;
    }

    public static ModelConfigurationBuilder HaveTextJsonConversions<TValue>(
        this ModelConfigurationBuilder configurationBuilder)
    {
        return configurationBuilder
            .HaveJsonConversion<TValue>()
            .HaveJsonConversion<List<TValue>>()
            .HaveJsonConversion<Dictionary<string, TValue>>()
            .HaveTextColumnTypes<TValue>();
    }

    public static ModelConfigurationBuilder HaveTextColumnTypes<TValue>(
        this ModelConfigurationBuilder modelConfigurationBuilder)
    {
        return modelConfigurationBuilder
            .HaveTextColumnType<TValue>()
            .HaveTextColumnType<List<TValue>>()
            .HaveTextColumnType<Dictionary<string, TValue>>();
    }
}