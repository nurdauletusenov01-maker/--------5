using Microsoft.AspNetCore.Identity;

namespace IdentityApp.Models;

public class ApplicationUser : IdentityUser
{
    [PersonalData]
    public string FirstName { get; set; } = string.Empty;

    [PersonalData]
    public string LastName { get; set; } = string.Empty;
}
