using Acesso;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskGroupWeb.Helpers;
using TaskGroupWeb.Models;

namespace TaskGroupWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public DbContext _db { get; set; }
        public IDataProtector _protector { get; set; }
        public IConfiguration _configuration { get; set; }

        private IMapper _mapper;
        private readonly string _tipoAutenticacao;

        public HomeController(DbContext _db, IDataProtectionProvider protectionProvider, IConfiguration configuration, IMapper mapper)
        {
            this._db = _db;
            this._protector = protectionProvider.CreateProtector(configuration.GetSection("ChaveCriptografia").Value);
            this._configuration = configuration;
            this._mapper = mapper;

            _tipoAutenticacao = configuration.GetSection("TipoAuthenticacao").Value;
        }

        public IActionResult Index()
        {
            try
            {
                var projects = _db.DbProject.List();
                var projectsModel = _mapper.Map<IList<ProjectModel>>(projects);
                var indexModel = new IndexModel()
                {
                    projectModels = projectsModel.ToList()
                };

                return View(indexModel);
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                TempData[OperationResult.Error.ToString()] = "Ocorreu um erro ao carregar os projetos!";
                return View();
            }            
        }

        public IActionResult Users()
        {
            return RedirectToAction("Index", "Users");
        }

        public IActionResult Projects()
        {
            return RedirectToAction("Index", "Projects");
        }
    }
}
