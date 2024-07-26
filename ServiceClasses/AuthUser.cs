using Microsoft.AspNetCore.Identity;

namespace Server.ServiceClasses;

public class AuthUser : IdentityUser
{
    public List<string>? SettingsJson { get; set; }
}