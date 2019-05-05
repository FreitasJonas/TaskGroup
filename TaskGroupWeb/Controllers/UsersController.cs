using Acesso;
using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Objetos;
using System;
using System.Collections.Generic;
using TaskGroupWeb.Helpers;
using TaskGroupWeb.Models;

namespace TaskGroupWeb.Controllers
{
    public class UsersController : Controller
    {
        public DbContext _db { get; set; }
        public IDataProtector _protector { get; set; }
        public IConfiguration _configuration { get; set; }

        private IMapper _mapper;
        private string _tipoAutenticacao;

        public UsersController(DbContext _db, IDataProtectionProvider protectionProvider, IConfiguration configuration, IMapper mapper)
        {
            this._db = _db;
            this._configuration = configuration;
            this._protector = protectionProvider.CreateProtector(configuration.GetSection("ChaveCriptografia").Value);            
            this._mapper = mapper;

            _tipoAutenticacao = configuration.GetSection("TipoAuthenticacao").Value;
        }

        #region GET

        public IActionResult Index()
        {
            var users = _mapper.Map<IList<User>, IList<UserModel>>(_db.DbUser.List());
            return View(users);
        }

        public IActionResult Edit(int idUser)
        {
            try
            {
                var user = _db.DbUser.Select(idUser);
                return View(_mapper.Map<UserModel>(user));
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                TempData["Error"] = "Erro ao carregar usuário!";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        #endregion

        #region POST

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userDb = _db.DbUser.Select(userModel.userId);
                    if (userDb.password != userModel.password)
                    {
                        userModel.password = Crypter.GetMD5(userModel.password);
                    }

                    var user = _mapper.Map<User>(userModel);
                    _db.DbUser.Update(user);

                    TempData["Sucess"] = "Usuário atualizado com sucesso!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Por favor preencha todos os campos obrigatórios!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                TempData["Error"] = "Erro ao atualizar usuário!";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    #region valida e-mail cadastrado

                    var isValid = false;
                    _db.DbUser.FindFromLogin(userModel.login, out isValid);

                    if (!isValid)
                    {
                        TempData["Error"] = "Já existe um usuário cadastrado com este e-mail!";
                        return View(userModel);
                    }

                    #endregion

                    userModel.password = Crypter.GetMD5(userModel.password);

                    var user = _mapper.Map<User>(userModel);                    
                    _db.DbUser.Insert(user);

                    TempData["Sucess"] = "Usuário salvo com sucesso!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Por favor preencha todos os campos obrigatórios!";
                    return View(userModel);
                }
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                TempData["Error"] = "Erro ao salvar usuário";
                return View(userModel);
            }
        }

        #endregion
    }
}