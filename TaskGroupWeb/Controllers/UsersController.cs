using Acesso;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Objetos;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using TaskGroupWeb.Filters;
using TaskGroupWeb.Helpers;
using TaskGroupWeb.Models;
using static Objetos.DbEnumerators;

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
        [ServiceFilter(typeof(ClaimRequirementFilterAttribute()]
        public IActionResult Index(string message = "", OperationResult status = OperationResult.Success)
        {
            try
            {
                var users = _mapper.Map<IList<User>, IList<UserModel>>(_db.DbUser.List());

                TempData[status.ToString()] = message;
                return View(users);
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                TempData[OperationResult.Error.ToString()] = "Ocorreu um erro ao carregar usuários!";
                return RedirectToAction("Index", "Home");
            }            
        }

        public IActionResult Edit(string userId)
        {
            try
            {
                #region - Decrypt -

                var idUser = userId.DecryptUrl();

                #endregion

                var user = _db.DbUser.Select(idUser);
                var userModel = _mapper.Map<UserModel>(user);

                ArrangeDropDownToEdit(userModel);
                return View(userModel);
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                return RedirectToAction("Index", new { message = "Erro ao carregar usuário!", status = OperationResult.Error });
            }
        }

        public IActionResult Create()
        {
            ArrangeDropDownToCreate();
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

                    return Json(new
                    {
                        action = Url.Action("Index", new { message = "Usuário atualizado com sucesso!", status = OperationResult.Success }),
                        status = OperationResult.Success
                    });
                }
                else
                {
                    return Json(new
                    {
                        action = Url.Action("Index", new { message = "Por favor preencha todos os campos obrigatórios!", status = OperationResult.Error }),
                        status = OperationResult.Error
                    });
                }
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);

                return Json(new
                {
                    action = Url.Action("Index", new { message = "Erro ao atualizar usuário!", status = OperationResult.Error }),
                    status = OperationResult.Error
                });
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
                        TempData[OperationResult.Error.ToString()] = "Já existe um usuário cadastrado com este e-mail!";
                        return View(userModel);
                    }

                    #endregion

                    userModel.password = Crypter.GetMD5(userModel.password);

                    var user = _mapper.Map<User>(userModel);                    
                    _db.DbUser.Insert(user);

                    return RedirectToAction("Index", new { message = "Usuário salvo com sucesso!", status = OperationResult.Success });
                }
                else
                {
                    TempData[OperationResult.Error.ToString()] = "Por favor preencha todos os campos obrigatórios!";
                    return View(userModel);
                }
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                TempData[OperationResult.Error.ToString()] = "Erro ao salvar usuário";
                return View(userModel);
            }
        }

        #endregion

        #region - UTILS -

        private void ArrangeDropDownToCreate()
        {
            ViewBag.Status = HtmlHelpers.GetDropDownFromEnum<UserStatus>((int)UserStatus.Ativo);
            ViewBag.NivelAcesso = HtmlHelpers.GetDropDownFromEnum<UserAcesso>((int)UserAcesso.Usuario);
        }

        private void ArrangeDropDownToEdit(UserModel user)
        {
            ViewBag.Status = HtmlHelpers.GetDropDownFromEnum<UserStatus>((int)user.status);
            ViewBag.NivelAcesso = HtmlHelpers.GetDropDownFromEnum<UserAcesso>((int)user.acesso);
        }

        #endregion
    }
}