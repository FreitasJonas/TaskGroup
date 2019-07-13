using Acesso;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskGroupWeb.Helpers;
using TaskGroupWeb.Models;
using static Objetos.DbEnumerators;

namespace TaskGroupWeb.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        public DbContext _db { get; set; }
        public IDataProtector _protector { get; set; }
        public IConfiguration _configuration { get; set; }

        private IMapper _mapper;
        private readonly string _tipoAutenticacao;

        public TasksController(DbContext _db, IDataProtectionProvider protectionProvider, IConfiguration configuration, IMapper mapper)
        {
            this._db = _db;
            this._protector = protectionProvider.CreateProtector(configuration.GetSection("ChaveCriptografia").Value);
            this._configuration = configuration;
            this._mapper = mapper;

            _tipoAutenticacao = configuration.GetSection("TipoAuthenticacao").Value;
        }

        #region - GET -

        public IActionResult Index(string _projectId)
        {
            try
            {
                #region - Decrypt -

                var projectId = int.Parse(_projectId.DecryptUrl());

                #endregion

                if (projectId > 0)
                {
                    var projectModel = _mapper.Map<ProjectModel>(_db.DbProject.Select(projectId));
                    var tasks = _mapper.Map<IList<TaskModel>>(_db.DbTask.ListFromProject(projectId)).ToList();
                    projectModel.tasks = tasks;

                    if (!string.IsNullOrEmpty(projectModel.git))
                    {
                        ViewBag.HasRepositoryLink = true;
                    }
                    else
                    {
                        ViewBag.HasRepositoryLink = false;
                    }

                    MakeBreadCrumbToIndexView(projectModel);
                    return View(projectModel);
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { message = "Projeto não encontrado!", status = OperationResult.Error });
                }
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                return RedirectToAction("Index", "Home", new { message = "Ocorreu um erro ao carregar o projeto!", status = OperationResult.Error });
            }
        }

        public IActionResult Create(string _projectId)
        {
            try
            {
                #region - Decrypt -

                var projectId = int.Parse(_projectId.DecryptUrl());

                #endregion

                TaskModel taskModel = new TaskModel();
                taskModel.projectId = projectId;

                MakeBreadCrumbToCreateView(_mapper.Map<Project, ProjectModel>(_db.DbProject.Select(taskModel.projectId)));
                ArrangeDropDownToCreate();
                return View(taskModel);
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                return RedirectToAction("Index", "Home", new { message = "Ocorreu um erro ao carregar tarefa!", status = OperationResult.Error });
            }
        }

        public IActionResult Edit(string _taskId, string message = "")
        {
            try
            {
                #region - Decrypt -

                var taskId = int.Parse(_taskId.DecryptUrl());

                #endregion

                #region prepare data

                var task = _db.DbTask.Select(taskId);
                var messages = _db.DbMessage.List(task.taskId);

                var taskModel = _mapper.Map<TaskModel>(task);
                var messagesModel = _mapper.Map<IList<MessageModel>>(messages).ToList();
                taskModel.messages = messagesModel;

                foreach (var _message in messagesModel)
                {
                    _message.user = _mapper.Map<UserModel>(_db.DbUser.Select(_message.userId));
                }

                #endregion

                var user = _mapper.Map<UserModel>(_db.DbUser.Select(task.userOwnId));
                ArrangeDropDownToEdit(taskModel, user);

                taskModel.userOwn = user;

                if (!string.IsNullOrEmpty(message))
                {
                    TempData[OperationResult.Success.ToString()] = message;
                }

                MakeBreadCrumbToEditView(_mapper.Map<Project, ProjectModel>(_db.DbProject.Select(taskModel.projectId)), taskModel);
                return View(taskModel);
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                return RedirectToAction("Index", "Home", new { message = "Ocorreu um erro ao carregar tarefa!", status = OperationResult.Error });
            }
        }

        public IActionResult CloseProject(string _projectId)
        {
            try
            {
                #region - Decrypt -

                var projectId = int.Parse(_projectId.DecryptUrl());

                #endregion

                _db.DbProject.CloseProject(projectId);

                return Json(new
                {
                    action = Url.Action("Index", "Home", new { message = "Projeto fechado com sucesso!", status = OperationResult.Success }),
                    status = OperationResult.Success
                });
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);

                return Json(new
                {
                    message = "Ocorreu um erro inesperado!",
                    status = OperationResult.Error
                });
            }
        }

        #endregion

        #region - POST -

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaskModel taskModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var task = _mapper.Map<Task>(taskModel);
                    task.dateCreated = DateTime.Now;
                    task.taskId = _db.DbTask.Insert(task);
                    task.taskCode = TaskCodeGenerator.Generate(task.taskId);

                    _db.DbTask.Update(task);

                    return Json(new
                    {
                        action = Url.Action("Index", new { _projectId = task.projectId.EncryptUrl() }),
                        status = OperationResult.Success
                    });
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
                    message = "Ocorreu um erro ao salvar tarefa!",
                    status = OperationResult.Error
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TaskModel taskModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var task = _mapper.Map<Task>(taskModel);
                    _db.DbTask.Update(task);

                    return Json(new
                    {
                        action = Url.Action("Edit", "Tasks", new { _taskId = task.taskId.EncryptUrl(), message = "Tarefa salva com sucesso!" }),
                        status = OperationResult.Success
                    });
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
                    message = "Ocorreu um erro ao carregar tarefa!",
                    status = OperationResult.Error
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendMessage(MessageModel messageModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var message = _mapper.Map<Message>(messageModel);
                    message.userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId").Value);
                    message.dateCreated = DateTime.Now;

                    _db.DbMessage.Insert(message);

                    return Json(new
                    {
                        action = Url.Action("Edit", "Tasks", new { _taskId = messageModel.taskId.EncryptUrl(), message = "Mensagem enviada com sucesso!" }),
                        status = OperationResult.Success
                    });

                    //if (UserUtilities.UserIsTaskOwner(User.Claims, _db.DbTask.Select(messageModel.taskId)))
                    //{
                        
                    //}
                    //else
                    //{
                    //    return Json(new { status = OperationResult.Error, message = "Você não possui permissão para executar esta ação!" });
                    //}
                }
                else
                {
                    return Json(new { status = OperationResult.Error, message = "Por favor preencha todos os campos obrigatórios!" });
                }
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                return Json(new { status = OperationResult.Error, message = "Ocorreu um erro ao enviar mensagem!" });
            }
        }

        #endregion

        #region - UTILS -

        private void ArrangeDropDownToCreate()
        {
            ViewBag.Status = HtmlHelpers.GetDropDownFromEnum<TaskStatus>((int)TaskStatus.Aberto);
            ViewBag.Users = HtmlHelpers.GetDropDownList(_db.DbUser.List(), "userId", "name");
            ViewBag.Projects = HtmlHelpers.GetDropDownList(_db.DbProject.List(), "projectId", "name");
        }

        private void ArrangeDropDownToEdit(TaskModel task, UserModel user)
        {
            ViewBag.Status = HtmlHelpers.GetDropDownFromEnum<TaskStatus>((int)task.status);
            ViewBag.Users = HtmlHelpers.GetDropDownList(_db.DbUser.List(), "userId", "name", user.name);
        }

        private void MakeBreadCrumbToIndexView(ProjectModel project)
        {
            var item1 = new BreadCrumbItem(project.name, Url.Action("Index", new { _projectId = project.projectId.EncryptUrl() }));
            var item2 = new BreadCrumbItem("Tarefas", "#");

            TempData["breadcrumb"] = HtmlHelpers.GetBreadCrumb(item1, item2);
        }

        private void MakeBreadCrumbToCreateView(ProjectModel project)
        {
            var item1 = new BreadCrumbItem(project.name, Url.Action("Index", new { _projectId = project.projectId.EncryptUrl() }));
            var item2 = new BreadCrumbItem("Nova Tarefa", "#");

            TempData["breadcrumb"] = HtmlHelpers.GetBreadCrumb(item1, item2);
        }

        private void MakeBreadCrumbToEditView(ProjectModel project, TaskModel task)
        {
            var item1 = new BreadCrumbItem(project.name, Url.Action("Index", new { _projectId = project.projectId.EncryptUrl() }));
            var item2 = new BreadCrumbItem(task.taskCode, Url.Action("Edit", new { _taskId = task.taskId.EncryptUrl() }));

            TempData["breadcrumb"] = HtmlHelpers.GetBreadCrumb(item1, item2);
        }

        #endregion
    }
}