namespace App.BLL.DTO.Exceptions.Identity;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string username) : base($"User with username {username} not found")
    {
    }

    public UserNotFoundException() : base("User not found")
    {
    }
}