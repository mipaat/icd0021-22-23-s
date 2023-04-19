using System.ComponentModel.DataAnnotations;

namespace Public.DTO.v1.Domain;

public class Game
{
    [MaxLength(16)] public string IgdbId { get; set; } = default!;
    [MaxLength(512)] public string Name { get; set; } = default!;
    [MaxLength(4096)] public string? BoxArtUrl { get; set; }
    
    [MaxLength(4096)] public string? Etag { get; set; }
    public DateTime LastFetched { get; set; }
    public DateTime? LastSuccessfulFetch { get; set; }

    public virtual void FromDomainEntity(App.Domain.Game entity)
    {
        
            IgdbId = entity.IgdbId;
            Name = entity.Name;
            BoxArtUrl = entity.BoxArtUrl;
            Etag = entity.Etag;
            LastFetched = entity.LastFetched;
            LastSuccessfulFetch = entity.LastSuccessfulFetch;
    }

    public virtual App.Domain.Game ToDomainEntity()
    {
        return new App.Domain.Game
        {
            Id = new Guid(),
            IgdbId = IgdbId,
            Name = Name,
            BoxArtUrl = BoxArtUrl,
            Etag = Etag,
            LastFetched = LastFetched,
            LastSuccessfulFetch = LastSuccessfulFetch,
        };
    }
}

public class GameWithId : Game
{
    public Guid Id { get; set; }

    public override void FromDomainEntity(App.Domain.Game entity)
    {
        base.FromDomainEntity(entity);
        Id = entity.Id;
    }

    public override App.Domain.Game ToDomainEntity()
    {
        var result = base.ToDomainEntity();
        result.Id = Id;
        return result;
    }
}