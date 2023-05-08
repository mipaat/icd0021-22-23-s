using App.Domain.Comparers;
using App.Domain.Converters;
using App.Domain.NotMapped;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL;

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
            .HaveJsonBConversions<VideoFile>()
            .HaveJsonBConversions<Caption>()
            .HaveJsonBColumnType<LangString>()
            .HaveJsonBColumnType<CaptionsDictionary>();
        configurationBuilder.Properties<CaptionsDictionary>()
            .HaveConversion<JsonValueConverter<CaptionsDictionary>, CaptionsDictionaryValueComparer>();
    }
}