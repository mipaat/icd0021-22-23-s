namespace Contracts.DAL;

public class FailedToResolveConcurrencyException : Exception
{
    public FailedToResolveConcurrencyException()
    {
    }

    public FailedToResolveConcurrencyException(Exception innerException) :
        base($"Failed to resolve concurrency exception", innerException)
    {
    }
}