namespace App.DAL.DTO.Entities;

public class CategoryWithCreatorAndVideoAssignments : CategoryWithCreator
{
    public ICollection<VideoCategoryOnlyIds> VideoCategories { get; set; } = default!;
}