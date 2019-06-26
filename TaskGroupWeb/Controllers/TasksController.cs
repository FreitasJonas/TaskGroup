using Acesso;
using AutoMapper;
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

        public IActionResult Index(int projectId)
        {
            try
            {
                if (projectId > 0)
                {
                    var projectModel = _mapper.Map<ProjectModel>(_db.DbProject.Select(projectId));
                    var tasks = _mapper.Map<IList<TaskModel>>(_db.DbTask.ListFromProject(projectId)).ToList();
                    projectModel.tasks = tasks;

                    return View(projectModel);
                }
                else
                {
                    TempData[OperationResult.Error.ToString()] = "Projeto não encontrado!";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                TempData[OperationResult.Error.ToString()] = "Ocorreu um erro ao carregar os projetos!";
                return View();
            }
        }

        public IActionResult Create(int projectId)
        {
            try
            {
                TaskModel taskModel = new TaskModel();
                taskModel.projectId = projectId;

                ArrangeDropDownToCreate();
                return View(taskModel);
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                TempData[OperationResult.Error.ToString()] = "Ocorreu um erro inesperado!";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Edit(int taskId, string message = "")
        {
            try
            {
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

                if (!string.IsNullOrEmpty(message))
                {
                    TempData[OperationResult.Success.ToString()] = message;
                }

                return View(taskModel);
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                TempData[OperationResult.Error.ToString()] = "Ocorreu um erro ao carregar tarefa!";
                return RedirectToAction("Index");
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

                    TempData[OperationResult.Success.ToString()] = "Tarefa salva com sucesso!";
                    return RedirectToAction("Index", new { projectId = task.projectId });
                }
                else
                {
                    ArrangeDropDownToCreate();
                    TempData[OperationResult.Error.ToString()] = "Por favor preencha todos os campos obrigatórios!";
                    return View(taskModel);
                }
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                TempData[OperationResult.Error.ToString()] = "Ocorreu um erro ao salvar tarefa!";
                return RedirectToAction("Index");
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
                    if (UserUtilities.UserIsTaskOwner(User.Claims, taskModel))
                    {
                        var task = _mapper.Map<Task>(taskModel);
                        _db.DbTask.Update(task);

                        return Json(new
                        {
                            action = Url.Action("Edit", "Tasks", new { taskId = task.taskId, message = "Tarefa salva com sucesso!" }),
                            status = OperationResult.Success
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            message = "Você não possui permissão para executar esta ação!",
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
                    if (UserUtilities.UserIsTaskOwner(User.Claims, _db.DbTask.Select(messageModel.taskId)))
                    {
                        var message = _mapper.Map<Message>(messageModel);
                        message.userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId").Value);
                        message.dateCreated = DateTime.Now;

                        _db.DbMessage.Insert(message);

                        return Json(new
                        {
                            action = Url.Action("Edit", "Tasks", new { taskId = messageModel.taskId, message = "Mensagem enviada com sucesso!" }),
                            status = OperationResult.Success
                        });
                    }
                    else
                    {
                        return Json(new { status = OperationResult.Error, message = "Você não possui permissão para executar esta ação!" });
                    }
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
            ViewBag.Status = HtmlDropDownHelper.GetDropDownFromEnum<TaskStatus>((int)TaskStatus.Aberto);
            ViewBag.Users = HtmlDropDownHelper.GetDropDownList(_db.DbUser.List(), "userId", "name");
            ViewBag.Projects = HtmlDropDownHelper.GetDropDownList(_db.DbProject.List(), "projectId", "name");
        }

        private void ArrangeDropDownToEdit(TaskModel task, UserModel user)
        {
            ViewBag.Status = HtmlDropDownHelper.GetDropDownFromEnum<TaskStatus>((int)task.status);
            ViewBag.Users = HtmlDropDownHelper.GetDropDownList(_db.DbUser.List(), "userId", "name", user.name);
        }

        #endregion
    }
}