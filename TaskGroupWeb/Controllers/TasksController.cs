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

        public IActionResult Index()
        {
            try
            {
                var projects = _db.DbProject.List();
                var _projects = _mapper.Map<IList<ProjectModel>>(_db.DbProject.List());

                foreach (var project in _projects)
                {
                    var _tasks = _db.DbTask.ListFromProject(project.projectId);
                    project.tasks = _mapper.Map<IList<TaskModel>>(_tasks).ToList();
                }

                return View(_projects);
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                TempData[OperationResult.Error.ToString()] = "Ocorreu um erro ao carregar os projetos!";
                return View();
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

        public IActionResult Edit(int taskId)
        {
            try
            {
                var task = _db.DbTask.Select(taskId);
                //var messages = _db.DbMessage.List(task.taskId);
                
                var taskModel = _mapper.Map<TaskModel>(task);
                //var messagesModel = _mapper.Map<IList<MessageModel>>(messages).ToList();
                taskModel.messages = new List<MessageModel>(); //messagesModel;

                var user = _mapper.Map<UserModel>(_db.DbUser.Select(task.userOwnId));
                ArrangeDropDownToEdit(taskModel, user);
                return View(taskModel);
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                TempData[OperationResult.Error.ToString()] = "Ocorreu um erro ao carregar tarefa!";
                return RedirectToAction("Index");
            }
        }

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
                    return RedirectToAction("Index");
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
                    var task = _mapper.Map<Task>(taskModel);
                    _db.DbTask.Update(task);

                    TempData[OperationResult.Success.ToString()] = "Tarefa salva com sucesso!";
                    return RedirectToAction("Index");
                }
                else
                {
                    var user = _mapper.Map<UserModel>(_db.DbUser.Select(taskModel.userOwnId));
                    ArrangeDropDownToEdit(taskModel, user);
                    TempData[OperationResult.Error.ToString()] = "Por favor preencha todos os campos obrigatórios!";
                    return View(taskModel);
                }
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                TempData[OperationResult.Error.ToString()] = "Ocorreu um erro ao carregar tarefa!";
                return RedirectToAction("Index");
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
                        action = Url.Action("Edit", "Tasks", new { taskId = messageModel.taskId }),
                        message = "Mensagem enviada com sucesso!",
                        status = OperationResult.Success
                    });
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
    }
}