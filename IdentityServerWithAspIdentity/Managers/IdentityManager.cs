using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServerWithAspIdentity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerWithAspIdentity.Managers
{
    public class IdentityManager : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityManager(UserManager<ApplicationUser> userManager, 
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        {
            _claimsFactory = claimsFactory;
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.FindByNameAsync(context.Subject.Identity.Name);
            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
            claims.Add(new System.Security.Claims.Claim(JwtClaimTypes.GivenName, user.UserName));
            claims.Add(new System.Security.Claims.Claim(IdentityServerConstants.StandardScopes.Email, user.Email));
            claims.Add(new System.Security.Claims.Claim(JwtClaimTypes.Id, user.TenantId));
            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.FindByNameAsync(context.Subject.Identity.Name);
            context.IsActive = user != null;
        }
    }
}
