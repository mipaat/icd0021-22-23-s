namespace App.BLL.DTO.Exceptions.Identity;

public class WrongPasswordException : Exception
{
    public WrongPasswordException(string username) : base($"Wrong password provided for user {username}")
    {
    }
}