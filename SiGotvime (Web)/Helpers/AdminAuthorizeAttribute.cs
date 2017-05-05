using SiGotvime.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SiGotvime__Web_.Helpers
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Account", action = "Login" })
                );
            }
            //User is logged in but has no access
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Home", action = "Index" })
                );
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);

            if (isAuthorized)
            {
                isAuthorized = Env.IsInRole(Constants.UserRoles.Administrator);
            }

            return isAuthorized;
        }
    }
}