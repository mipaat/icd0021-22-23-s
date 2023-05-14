namespace App.DAL.DTO.Entities;

public class VideoWithComments : Video
{
    public ICollection<Comment> Comments { get; set; } = default!;
}