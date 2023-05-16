namespace App.BLL.DTO.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public string Text { get; set; } = default!;
    public Author Author { get; set; } = default!;
    public DateTime? CreatedAt { get; set; }
}