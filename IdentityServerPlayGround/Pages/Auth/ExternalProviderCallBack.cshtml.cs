using IdentityServer4.Services;
using IdentityServerPlayGround.EF;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace IdentityServerPlayGround.Pages.Auth
{
    public class ExternalProviderCallBackModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public ExternalProviderCallBackModel( SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<ActionResult> OnGet()
        {
                var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
                if (externalLoginInfo == null)
                    throw new Exception();
                var result = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.ProviderDisplayName, externalLoginInfo.ProviderKey, false);
                if (result.Succeeded)
                    return Redirect(ReturnUrl);
            //Add User To Our Provider
            var user = new ApplicationUser();
            user.UserName = externalLoginInfo.Principal.FindFirst(c=>c.Type == "name")?.Value?.Replace(" ","_");
            var cratingResult = await _userManager.CreateAsync(user);
            if (cratingResult.Succeeded)
            {
                await _userManager.AddLoginAsync(user, externalLoginInfo);
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            return Redirect(ReturnUrl);

        }
    }
}
