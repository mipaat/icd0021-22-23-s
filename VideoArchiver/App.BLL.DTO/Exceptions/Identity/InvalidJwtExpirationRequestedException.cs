namespace App.BLL.DTO.Exceptions.Identity;

public class InvalidJwtExpirationRequestedException : Exception
{
    public readonly int ExpiresInSeconds;

    public InvalidJwtExpirationRequestedException(int expiresInSeconds) : base(
        $"Requested JWT expiration time must be greater than 0 seconds, but was {expiresInSeconds}")
    {
        ExpiresInSeconds = expiresInSeconds;
    }
}