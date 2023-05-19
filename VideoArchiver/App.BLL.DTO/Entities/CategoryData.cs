using App.Common;
using App.Common.Validation;

namespace App.BLL.DTO.Entities;

public class CategoryData
{
    [LangStringNotEmpty] public LangString Name { get; set; } = default!;
    public bool IsPublic { get; set; }
}