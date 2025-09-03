using Microsoft.AspNetCore.Identity;

namespace WeddingApp.Identity;

public class WeddingAppUser: IdentityUser
{
    public string? Name {get; set;}
}