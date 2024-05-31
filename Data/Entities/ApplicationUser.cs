using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class ApplicationUser : IdentityUser
{
    [EmailAddress]
    public string Email { get; set; }

    public string? Role { get; set; }
}
