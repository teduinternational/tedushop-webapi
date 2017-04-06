using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;
using TeduShop.Model.Models;

namespace TeduShop.Web.Providers
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            await Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

            if (allowedOrigin == null) allowedOrigin = "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            UserManager<AppUser> userManager = context.OwinContext.GetUserManager<UserManager<AppUser>>();
            AppUser user;
            try
            {
                user = await userManager.FindAsync(context.UserName, context.Password);
            }
            catch
            {
                // Could not retrieve the user due to error.
                context.SetError("server_error");
                context.Rejected();
                return;
            }
            if (user != null)
            {
                ClaimsIdentity identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ExternalBearer);
                identity.AddClaim(new Claim("fullName", user.FullName));
                identity.AddClaim(new Claim("avatar", user.Avatar));
                identity.AddClaim(new Claim("email", user.Email));

                context.Validated(identity);
            }
            else
            {
                context.SetError("invalid_grant", "Tài khoản hoặc mật khẩu không đúng.'");
                context.Rejected();
            }
        }
    }
}