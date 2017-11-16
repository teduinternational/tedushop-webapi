using Microsoft.AspNet.SignalR;
using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using TeduShop.Web.Mappings;

namespace TeduShop.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutoMapperConfiguration.Configure();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Context.Response.AppendHeader("Access-Control-Allow-Credentials", "true");
            var referrer = Request.UrlReferrer;
            if (Context.Request.Path.Contains("signalr/") && referrer != null)
            {
                Context.Response.AppendHeader("Access-Control-Allow-Origin", referrer.Scheme + "://" + referrer.Authority);
            }
        }
    }
}