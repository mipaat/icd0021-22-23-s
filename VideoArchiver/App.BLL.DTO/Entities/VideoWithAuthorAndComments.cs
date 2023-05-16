namespace App.BLL.DTO.Entities;

public class VideoWithAuthorAndComments : VideoWithAuthor
{
    public ICollection<Comment> Comments { get; set; } = default!;
}