using System.Net;

namespace Public.DTO.v1;

/// <summary>
/// Information about an error that occurred while responding to a request to the API.
/// </summary>
public class RestApiErrorResponse
{
    /// <summary>
    /// The HTTP status code identifying the type of error response.
    /// </summary>
    public HttpStatusCode Status { get; set; }
    /// <summary>
    /// Text describing the error.
    /// </summary>
    public string Error { get; set; } = default!;
}