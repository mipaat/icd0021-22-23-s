using Domain.Converters;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public static class ConfigurationBuilderExtensions {
    public const string JsonB = "jsonb";
    public const string Text = "TEXT";

    public static ModelConfigurationBuilder HaveJsonConversion<TValue>(
        this ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<TValue>()
            .HaveConversion<JsonConverter<TValue>>();
        return configurationBuilder;
    }

    public static ModelConfigurationBuilder HaveJsonBColumnType<TValue>(this ModelConfigurationBuilder configurationBuilder)
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
    
    public static ModelConfigurationBuilder HaveTextColumnType<TValue>(this ModelConfigurationBuilder configurationBuilder)
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