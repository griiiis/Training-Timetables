using System.ComponentModel.DataAnnotations;
using App.Domain.Enums;

namespace App.DTO.v1_0.Identity;

public class RegisterInfo
{
    [StringLength(128, MinimumLength = 1, ErrorMessage = "Incorrect length")]
    public string Email { get; set; } = default!;
    
    [StringLength(128, MinimumLength = 1, ErrorMessage = "Incorrect length")]
    public string Password { get; set; } = default!;
    
    [StringLength(128, MinimumLength = 1, ErrorMessage = "Incorrect length")]
    public string Firstname { get; set; } = default!;

    [StringLength(128, MinimumLength = 1, ErrorMessage = "Incorrect length")]
    public string Lastname { get; set; } = default!;
    public int Age { get; set; } = default!;
    public EGender Gender { get; set; } = default!;
    
}