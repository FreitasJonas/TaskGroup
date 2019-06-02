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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProjectModel projectModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (projectModel.usersSubscribe.Count > 0)
                    {
                        var project = _mapper.Map<Project>(projectModel);
                        project.authorId = UserUtilities.GetUserId(User.Claims);

                        project.projectId = _db.DbProject.Insert(project);

                        foreach (var userSubscribe in projectModel.usersSubscribe)
                        {
                            _db.DbProject.InsertUserSubscribe(project, userSubscribe.userId);
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
                    var project = _mapper.Map<Project>(projectModel);
                    _db.DbProject.Update(project);

                    TempData[OperationResult.Success.ToString()] = "Projeto salvo com sucesso!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ArrangeDropDownToEdit(projectModel);
                    TempData[OperationResult.Error.ToString()] = "Por favor preencha todos os campos obrigatórios!";
                    return View(projectModel);
                }
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                TempData[OperationResult.Error.ToString()] = "Ocorreu um erro ao carregar projeto!";
                return RedirectToAction("Index");
            }

        }

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
        }
    }
}