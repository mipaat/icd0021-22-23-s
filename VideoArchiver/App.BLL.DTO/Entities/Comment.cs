namespace App.BLL.DTO.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public string Content { get; set; } = default!;
    public Author Author { get; set; } = default!;
    public DateTime? CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<Comment> Replies { get; set; } = default!;

    public int? LikeCount { get; set; }
    public int? DislikeCount { get; set; }
    public bool? IsFavorited { get; set; }
    
    public DateTime? LastFetchOfficial { get; set; }
    public DateTime? LastSuccessfulFetchOfficial { get; set; }
    public DateTime? LastFetchUnofficial { get; set; }
    public DateTime? LastSuccessfulFetchUnofficial { get; set; }
    public DateTime AddedToArchiveAt { get; set; }
}