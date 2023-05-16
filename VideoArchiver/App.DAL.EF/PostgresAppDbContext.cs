using App.Common;
using App.Domain.Comparers;
using App.Domain.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace App.DAL.EF;

public class PostgresAppDbContext : AbstractAppDbContext
{
    public PostgresAppDbContext(DbContextOptions<PostgresAppDbContext> options, IConfiguration configuration) : base(options, configuration)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder
            .HaveJsonBColumnType<ImageFileList>()
            .HaveConversionAndComparer<ImageFileList, JsonValueConverter<ImageFileList>, ImageFileListValueComparer>()
            .HaveJsonBColumnType<List<VideoFile>>()
            .HaveJsonConversion<List<VideoFile>>()
            .HaveJsonBConversions<Caption>()
            .HaveJsonBColumnType<LangString>()
            .HaveJsonBColumnType<CaptionsDictionary>();
        configurationBuilder.Properties<CaptionsDictionary>()
            .HaveConversion<JsonValueConverter<CaptionsDictionary>, CaptionsDictionaryValueComparer>();
    }
}