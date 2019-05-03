using Acesso;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskGroupWeb.Models;

namespace TaskGroupWeb.Controllers
{
    public class LoginController : Controller
    {
        public DbContext _db { get; set; }
        public IDataProtector _protector { get; set; }
        public IConfiguration _configuration { get; set; }

        public LoginController(DbContext _db, IDataProtectionProvider protectionProvider, IConfiguration configuration)
        {
            this._db = _db;
            this._protector = protectionProvider.CreateProtector(configuration.GetSection("ChaveCriptografia").Value);
            this._configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserLogin([Bind] LoginModel user)
        {
            if (ModelState.IsValid)
            {
                var LoginStatus = _db.userAcesso.ValidateUser(user.login, user.senha); 

                if (LoginStatus)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.login)
                    };

                    ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                    await HttpContext.SignInAsync(principal);

                    TempData["Message"] = "Success!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "Login Failed.Please enter correct credentials";
                    return View();
                }
            }
            else { 
                return View();
            }
        }
    }
}