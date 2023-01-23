using IdentityServerPlayGround.EF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServerPlayGround.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController( UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost(Name = "NewUser")]
        public async Task<ActionResult> Post()
        {
            var isSuccedded = await _userManager.CreateAsync(new ApplicationUser { UserName = "emad", Email = "emad@emad.com" }, "P@ssw0rd");
            if (isSuccedded.Succeeded)
            {
                var selectedUser =await  _userManager.FindByNameAsync("emad");
                await _userManager.AddClaimAsync(selectedUser, new System.Security.Claims.Claim("SSN", "123456"));
                await _userManager.AddClaimAsync(selectedUser, new System.Security.Claims.Claim("Address", "108 St 4"));
            }
            return Ok();
        }
        [HttpGet(Name = "Login")]
        public async Task<ActionResult> Login(string ReturnUrl)
        {
            //Return Login Page
            //Then Use This Login
            var result = await _signInManager.PasswordSignInAsync("emad","P@ssw0rd", false, lockoutOnFailure: true);
            return Redirect(ReturnUrl);
        }
        [HttpGet(Name = "Logout")]
        public async Task<ActionResult> Logout(string LogoutId)
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/");
        }


    }
}
