using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Objetos.DbEnumerators;

namespace TaskGroupWeb.Filters
{
    public class CustomActionFilter : IAuthorizationFilter
    {
        readonly List<string> perfisAllowed;

        public CustomActionFilter(params UserAcesso[] perfis)
        {
            foreach (var perfil in perfis)
            {
                perfisAllowed.Add(perfil.ToString());
            }
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var acesso = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "acesso");

            if (acesso != null)
            {
                
            }
            else
            {
                
            }
        }
    }
}
