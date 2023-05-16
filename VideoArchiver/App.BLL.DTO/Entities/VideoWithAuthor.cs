namespace App.BLL.DTO.Entities;

public class VideoWithAuthor : Video
{
    public Author Author { get; set; } = default!;
}