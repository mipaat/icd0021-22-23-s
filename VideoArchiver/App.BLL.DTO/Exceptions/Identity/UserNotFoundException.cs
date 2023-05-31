using App.Common;
using App.Common.Exceptions;

namespace App.BLL.DTO.Exceptions.Identity;

public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(string username) : base($"User with username {username} not found")
    {
    }

    public UserNotFoundException() : base("User not found")
    {
    }
}