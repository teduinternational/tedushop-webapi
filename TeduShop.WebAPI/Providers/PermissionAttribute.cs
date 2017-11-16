using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;
using TeduShop.Common.Enums;
using TeduShop.Web.Models;

namespace TeduShop.Web.Providers
{
    public class PermissionAttribute : AuthorizeAttribute
    {
        public string Function;
        public string Action;

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;

            if (!principal.Identity.IsAuthenticated)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            else
            {
                var roles = JsonConvert.DeserializeObject<List<string>>(principal.FindFirst("roles").Value);
                if (roles.Count > 0)
                {
                    if (!roles.Contains(RoleEnum.Admin.ToString()))
                    {
                        var permissions = JsonConvert.DeserializeObject<List<PermissionViewModel>>(principal.FindFirst("permissions").Value);
                        if (!permissions.Exists(x => x.FunctionId == Function && x.CanCreate) && Action == ActionEnum.Create.ToString())
                        {
                            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);

                        }
                        else if (!permissions.Exists(x => x.FunctionId == Function && x.CanRead) && Action == ActionEnum.Read.ToString())
                        {
                            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);

                        }
                        else if (!permissions.Exists(x => x.FunctionId == Function && x.CanDelete) && Action == ActionEnum.Delete.ToString())
                        {
                            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);

                        }
                        else if (!permissions.Exists(x => x.FunctionId == Function && x.CanUpdate) && Action == ActionEnum.Update.ToString())
                        {
                            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                        }
                    }
                }
                else
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                }
            }
        }
    }
}