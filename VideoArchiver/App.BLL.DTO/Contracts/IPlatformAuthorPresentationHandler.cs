using App.BLL.DTO.Entities;

namespace App.BLL.DTO.Contracts;

public interface IPlatformAuthorPresentationHandler
{
    public bool CanHandle(Author author);
    public Author Handle(Author author);
}