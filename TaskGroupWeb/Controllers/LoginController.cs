using Acesso;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Objetos;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskGroupWeb.Helpers;
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
        public async Task<IActionResult> UserLogin(LoginModel _login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _login.password = Crypter.GetMD5(_login.password);

                    var LoginStatus = false;
                    var _user = _db.DbUser.ValidateUser(_login.login, _login.password, out LoginStatus);

                    if (LoginStatus)
                    {
                        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, _user.name),
                        new Claim("email", _user.login),
                        new Claim("contact", _user.contact),
                        new Claim("date.created", _user.dateCreated.ToString()),
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
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["Message"] = "Por favor preencha todos os campos!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                TempData["Message"] = "Ocorreu um erro inesperado!";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Logout()
        {
            TempData.Clear();
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}