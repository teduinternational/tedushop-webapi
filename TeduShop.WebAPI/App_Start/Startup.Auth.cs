using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using TeduShop.Data;
using TeduShop.Identity;
using TeduShop.Web.Providers;

[assembly: OwinStartup(typeof(TeduShop.Web.App_Start.Startup))]

namespace TeduShop.Web.App_Start
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(TeduShopDbContext.Create);

            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/oauth/token"),
                Provider = new AuthorizationServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                AllowInsecureHttp = true,
            });
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
        }
    }
}