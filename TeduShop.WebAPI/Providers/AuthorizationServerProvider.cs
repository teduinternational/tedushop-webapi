using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TeduShop.Model.Models;

namespace TeduShop.Web.Providers
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public AuthorizationServerProvider()
        {

        }
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
                context.SetError("server_error", "Lỗi trong quá trình xử lý.");
                context.Rejected();
                return;
            }
            if (user != null)
            {
                ClaimsIdentity identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ExternalBearer);
                string avatar = string.IsNullOrEmpty(user.Avatar) ? "" : user.Avatar;
                string email  = string.IsNullOrEmpty(user.Email) ? "" : user.Email;
                identity.AddClaim(new Claim("fullName", user.FullName));
                identity.AddClaim(new Claim("avatar", avatar));
                identity.AddClaim(new Claim("email", email));
                var props = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        {"fullName", user.FullName},
                        {"avatar", avatar },
                        {"email", email}
                    });
                context.Validated(new AuthenticationTicket(identity, props));
            }
            else
            {
                context.SetError("invalid_grant", "Tài khoản hoặc mật khẩu không đúng.'");
                context.Rejected();
            }
        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }

    }
}