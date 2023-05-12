using App.BLL.DTO.Enums;

namespace App.BLL.DTO.Entities;

public class Author
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? DisplayName { get; set; }
    public Platform Platform { get; set; } = default!;
}