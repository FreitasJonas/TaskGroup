using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TaskGroupWeb.Filters
{
    public class ClaimRequirementFilterAttribute : IAuthorizationFilter
    {
        readonly Claim _claim;

        public ClaimRequirementFilterAttribute(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == _claim.Type && c.Value == _claim.Value);
            if (!hasClaim)
            {
                context.Result = new ContentResult()
                {
                    Content = "Acesso negado!"
                };
            }
        }
    }
}
