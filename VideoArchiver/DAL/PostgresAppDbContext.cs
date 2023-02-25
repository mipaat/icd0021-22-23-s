using Domain.NotMapped;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class PostgresAppDbContext : AbstractAppDbContext
{
    public PostgresAppDbContext(DbContextOptions<PostgresAppDbContext> options) : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder
            .HaveJsonBConversions<ImageFile>()
            .HaveJsonBConversions<VideoFile>()
            .HaveJsonBConversions<Caption>()
            .HaveJsonBColumnType<LangString>();
    }
}