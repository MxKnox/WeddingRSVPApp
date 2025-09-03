using Microsoft.AspNetCore.Identity;
using WeddingApp.Identity;

namespace WeddingApp.Components.Account;

internal sealed class WeddingAppUserAccessor(
    UserManager<WeddingAppUser> userManager,
    IdentityRedirectManager redirectManager)
{
    public async Task<WeddingAppUser> GetRequiredUserAsync(HttpContext context)
    {
        var user = await userManager.GetUserAsync(context.User);

        if (user is null)
            redirectManager.RedirectToWithStatus("Account/InvalidUser",
                $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);

        return user;
    }
}