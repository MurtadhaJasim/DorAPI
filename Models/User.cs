using System;
using System.ComponentModel.DataAnnotations;

namespace Dor.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
    public string Name { get; set; } = default!;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    [Required]
    [StringLength(20)]
    public string Role { get; set; } = "User";

    // Refresh Token Support
    public string? RefreshToken { get; set; }

    [DataType(DataType.DateTime)]
    [Display(Name = "Token Expiry Time")]
    public DateTime? RefreshTokenExpiryTime { get; set; }
}