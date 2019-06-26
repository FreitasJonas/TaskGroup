using Acesso;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Objetos;
using System;
using System.Collections.Generic;
using TaskGroupWeb.Helpers;
using TaskGroupWeb.Models;
using System.Linq;

namespace TaskGroupWeb.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        public DbContext _db { get; set; }
        public IDataProtector _protector { get; set; }
        public IConfiguration _configuration { get; set; }

        private IMapper _mapper;
        private string _tipoAutenticacao;

        public ProjectsController(DbContext _db, IDataProtectionProvider protectionProvider, IConfiguration configuration, IMapper mapper)
        {
            this._db = _db;
            this._protector = protectionProvider.CreateProtector(configuration.GetSection("ChaveCriptografia").Value);
            this._configuration = configuration;
            this._mapper = mapper;

            _tipoAutenticacao = configuration.GetSection("TipoAuthenticacao").Value;
        }

        #region - GET - 

        public IActionResult Index(string message = "", OperationResult result = OperationResult.Success)
        {
            try
            {
                if (!string.IsNullOrEmpty(message))
                {
                    TempData[result.ToString()] = message;
                }

                var projects = _mapper.Map<IList<ProjectModel>>(_db.DbProject.List());
                return View(projects);
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                TempData[OperationResult.Error.ToString()] = "Ocorreu um erro ao carregar projetos!";
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Create()
        {
            try
            {
                ArrangeDropDownToCreate();
                return View();
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                TempData[OperationResult.Error.ToString()] = "Ocorreu um erro inesperado!";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Edit(int projectId)
        {
            try
            {
                var project = _db.DbProject.Select(projectId);
                var projectModel = _mapper.Map<ProjectModel>(project);
                projectModel.users = _mapper.Map<IList<User>, IList <UserModel>>(_db.DbProject.ListUsers(project.projectId)).ToList();

                ArrangeDropDownToEdit(projectModel);
                return View(projectModel);
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                TempData[OperationResult.Error.ToString()] = "Ocorreu um erro ao carregar projeto!";
                return RedirectToAction("Index");
            }
        }

        #endregion

        #region - POST -

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProjectModel projectModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var users = projectModel.usersSubscribeId.Split(";");

                    if (users.Length > 0)
                    {
                        var project = _mapper.Map<Project>(projectModel);
                        project.authorId = UserUtilities.GetUserId(User.Claims);

                        project.projectId = _db.DbProject.Insert(project);

                        foreach (var userSubscribeId in users)
                        {
                            _db.DbProject.InsertUserSubscribe(project, int.Parse(userSubscribeId));
                        }

                        return Json(new
                        {
                            action = Url.Action("Index", new { status = OperationResult.Success, message = "Projeto salvo com sucesso!" }),
                            status = OperationResult.Success
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            message = "É necessário adicionar os participantes!",
                            status = OperationResult.Error
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        message = "Por favor preencha todos os campos obrigatórios!",
                        status = OperationResult.Error
                    });
                }
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                return Json(new
                {
                    message = "Ocorreu um erro ao salvar projeto!",
                    status = OperationResult.Error
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProjectModel projectModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var users = projectModel.usersSubscribeId.Split(";");

                    if (users.Length > 0)
                    {
                        var project = _mapper.Map<Project>(projectModel);
                        _db.DbProject.Update(project);
                        _db.DbProject.UpdateUsers(project.projectId, users);

                        return Json(new
                        {
                            action = Url.Action("Index", 
                            new {
                                message = "Projeto salvo com sucesso!",
                                status = OperationResult.Success
                            }),
                            status = OperationResult.Success
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            message = "O projeto deve conter no mínimo um participante!",
                            status = OperationResult.Error
                        });
                    }                    
                }
                else
                {
                    ArrangeDropDownToEdit(projectModel);
                    return Json(new
                    {
                        message = "Por favor preencha todos os campos obrigatórios!",
                        status = OperationResult.Error
                    });
                }
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                TempData[OperationResult.Error.ToString()] = "Ocorreu um erro ao carregar projeto!";
                return RedirectToAction("Index");
            }
        }

        #endregion

        #region - UTILS -

        public IActionResult GetProjects()
        {
            try
            {
                var _projects = _db.DbProject.List();
                return Json(new { status = OperationResult.Success, projects = _projects });
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                return Json(new { status = OperationResult.Error, message = "Falha ao carregar projetos" });
            }
        }

        private void ArrangeDropDownToCreate()
        {
            ViewBag.FrameWorks = HtmlDropDownHelper.GetDropDownList(_db.DbParam.List("project_frameworks"), "value", "value");
            ViewBag.Users = HtmlDropDownHelper.GetDropDownList(_db.DbUser.List(), "userId", "name");
        }

        private void ArrangeDropDownToEdit(ProjectModel project)
        {
            ViewBag.FrameWorks = HtmlDropDownHelper.GetDropDownList(_db.DbParam.List("project_frameworks"), "value", "value", project.framework);
            ViewBag.Users = HtmlDropDownHelper.GetDropDownList(_db.DbUser.List(), "userId", "name");
        }

        #endregion
    }
}