using App.Common;

namespace App.BLL.YouTube.DTO;

public class BasicCategoryData
{
    public LangString Name { get; set; } = default!;
    public string IdOnPlatform { get; set; } = default!;
    public bool IsAssignable { get; set; }
}