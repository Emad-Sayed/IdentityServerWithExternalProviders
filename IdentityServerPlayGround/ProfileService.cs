using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Security.Claims;

namespace IdentityServerPlayGround
{
    public class ProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //Why IssuedClaims Empty!!
            context.IssuedClaims.Add(new Claim("test-claim", "test-value"));
            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;

            return Task.FromResult(0);
        }
    }
}
