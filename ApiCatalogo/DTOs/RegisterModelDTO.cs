using System.ComponentModel.DataAnnotations;

namespace ApiCatalogo.DTOs;

public class RegisterModelDTO
{
    [Microsoft.Build.Framework.Required]
    public string? UserName { get; set; }
    
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    
    
    [Required]
    public string? Password { get; set; }
    
}