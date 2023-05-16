namespace App.DAL.DTO.Entities;

public class VideoWithBasicAuthors : Video
{
    public ICollection<AuthorBasic> Authors { get; set; } = default!;
}