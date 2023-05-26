using Public.DTO.v1;
using Swashbuckle.AspNetCore.Annotations;
#pragma warning disable CS1591

namespace WebApp.MyLibraries;

public class SwaggerRestApiErrorResponseAttribute : SwaggerResponseAttribute
{
    public SwaggerRestApiErrorResponseAttribute(int statusCode) : base(statusCode, null, typeof(RestApiErrorResponse))
    {
    }
}