using System;
using System.ComponentModel.DataAnnotations;

namespace Dor.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Username is required")]
    public string Name { get; set; } = default!;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    [Required]
    [StringLength(20)]
    public string Role { get; set; } = "User";

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }
}