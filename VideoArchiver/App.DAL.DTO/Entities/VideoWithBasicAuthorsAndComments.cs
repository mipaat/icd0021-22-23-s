namespace App.DAL.DTO.Entities;

public class VideoWithBasicAuthorsAndComments : VideoWithBasicAuthors
{
    public ICollection<Comment> Comments { get; set; } = default!; // TODO: Replace with basic comments
}