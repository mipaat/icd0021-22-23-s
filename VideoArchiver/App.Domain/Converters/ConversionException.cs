namespace App.Domain.Converters;

public class ConversionException : Exception
{
    public ConversionException(string? message = null) : base(message)
    {
    }
}