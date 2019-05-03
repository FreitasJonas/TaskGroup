using Acesso;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Objetos;
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

        private IMapper _mapper;
        private string _tipoAutenticacao;

        public LoginController(DbContext _db, IDataProtectionProvider protectionProvider, IConfiguration configuration, IMapper mapper)
        {
            this._db = _db;
            this._protector = protectionProvider.CreateProtector(configuration.GetSection("ChaveCriptografia").Value);
            this._configuration = configuration;
            this._mapper = mapper;

            _tipoAutenticacao = configuration.GetSection("TipoAuthenticacao").Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserLogin([Bind] UserModel user)
        {
            if (ModelState.IsValid)
            {
                user.password = _protector.Protect(user.password);

                var model = _mapper.Map<User>(user);

                var LoginStatus = false;
                var _user = _db.userAcesso.ValidateUser(model, out LoginStatus); 

                if (LoginStatus)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.name),
                        new Claim("email", user.login),
                        new Claim("contact", user.contact),
                        new Claim("date.created", user.dateCreated.ToString()),
                    };

                    ClaimsIdentity userIdentity = new ClaimsIdentity(claims, _tipoAutenticacao);
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                    await HttpContext.SignInAsync(principal);

                    TempData["Message"] = "Success!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Message"] = "Usuário ou senha incorretos!";
                    return View();
                }
            }
            else { 
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}