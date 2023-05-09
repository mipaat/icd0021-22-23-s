using System.ComponentModel.DataAnnotations;

namespace Public.DTO.v1.Identity;

public class Login
{
    [StringLength(maximumLength:128, MinimumLength = 1, ErrorMessage = "Wrong length on username")] 
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
}