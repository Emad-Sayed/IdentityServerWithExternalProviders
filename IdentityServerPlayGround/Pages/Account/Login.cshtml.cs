using IdentityServer4.Services;
using IdentityServerPlayGround.EF;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Xml.Linq;

namespace IdentityServerPlayGround.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        [BindProperty( SupportsGet = true)]
        public string ReturnUrl { get; set; }
        [BindProperty]
        public string UserName { get; set; }
        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string ProviderName { get; set; }
        public List<AuthenticationScheme> ExternalProviders { get; set; }

        public LoginModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
        }
        public async Task OnGet()
        {
            ExternalProviders = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }
        public async Task<ActionResult> OnPost()
        {
            var result = await _signInManager.PasswordSignInAsync(UserName, Password, false, lockoutOnFailure: true);
            return Redirect(ReturnUrl);
        }
        public async Task<ActionResult> OnPostExternalLogin()
        {
            var redirectUrl = "/Account/ExternalProviderCallBack?ReturnUrl="+WebUtility.UrlEncode(ReturnUrl);
            var props =  _signInManager.ConfigureExternalAuthenticationProperties(ProviderName, redirectUrl);
            return Challenge(props);
        }
    }
}
