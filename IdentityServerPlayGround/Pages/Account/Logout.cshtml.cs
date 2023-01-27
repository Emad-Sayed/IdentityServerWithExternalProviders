using IdentityServer4.Services;
using IdentityServerPlayGround.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerPlayGround.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        public LogoutModel(SignInManager<ApplicationUser> signInManager, IIdentityServerInteractionService interaction)
        {
             _signInManager=signInManager;
            _interaction=interaction; 
        }
        [BindProperty(SupportsGet = true)]
        public string LogoutId { get; set; }
        public async Task <ActionResult> OnGet()
        {
            var logout = await _interaction.GetLogoutContextAsync(LogoutId);

            await _signInManager.SignOutAsync();


            return Redirect(logout?.PostLogoutRedirectUri);
        }
    }
}
