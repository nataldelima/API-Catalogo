using Microsoft.Build.Framework;

namespace ApiCatalogo.DTOs;

public class LoginModelDTO
{
    [Required]
    public string? UserName { get; set; }
    
    [Required]
    public string? Password { get; set; }
}