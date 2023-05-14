using App.DAL.DTO.Base;
using App.DAL.DTO.NotMapped;

namespace App.DAL.DTO.Entities;

public class Playlist : BaseArchiveEntity
{
    public LangString? Title { get; set; }
    public LangString? Description { get; set; }

    public string? DefaultLanguage { get; set; }

    public ImageFileList? Thumbnails { get; set; }
    public List<string>? Tags { get; set; }

    public DateTime? PublishedAt { get; set; }

    public DateTime? LastVideosFetch { get; set; }
}