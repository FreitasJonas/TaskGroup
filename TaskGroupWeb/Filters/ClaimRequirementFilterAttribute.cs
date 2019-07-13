using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace TaskGroupWeb.Filters
{
    public class ClaimRequirementFilterAttribute :  Attribute, IAuthorizationFilter
    {
        readonly string PerfilAllowed;

        public ClaimRequirementFilterAttribute(string perfilAllowed)
        {
            PerfilAllowed = perfilAllowed;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == "acesso" && c.Value == PerfilAllowed);
            if (!hasClaim)
            {
                context.Result = new RedirectToActionResult("Logout", "Login", new { message = "Acesso negado!" });
            }
        }
    }
}
