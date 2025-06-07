using System;
using System.ComponentModel.DataAnnotations;

namespace Dor.Models;

public class Customers
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Photo Path")]
    public string? PhotoPath { get; set; }

    [Display(Name = "Marital Status")]
    [StringLength(50)]
    public string? Marital_status { get; set; }

    [Required(ErrorMessage = "Nationality is required")]
    [StringLength(50)]
    public string Nationality { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Job { get; set; }

    [StringLength(100)]
    public string? Complex { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Display(Name = "House Number")]
    [StringLength(20)]
    public string? HouseNumber { get; set; }

    [DataType(DataType.DateTime)]
    [Display(Name = "Created At")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [DataType(DataType.DateTime)]
    [Display(Name = "Updated At")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [DataType(DataType.DateTime)]
    [Display(Name = "Deleted At")]
    public DateTime? DeletedAt { get; set; } = null;
}