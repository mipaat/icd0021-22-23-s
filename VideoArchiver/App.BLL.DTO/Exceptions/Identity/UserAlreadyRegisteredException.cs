namespace App.BLL.DTO.Exceptions.Identity;

public class UserAlreadyRegisteredException : Exception
{
    public readonly string UserName;
    
    public UserAlreadyRegisteredException(string userName) : base(
        $"User with username {userName} is already registered")
    {
        UserName = userName;
    }
}