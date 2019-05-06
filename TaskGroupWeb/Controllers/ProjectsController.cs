﻿using Acesso;
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

        public IActionResult Index()
        {
            try
            {
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
                    var project = _mapper.Map<Project>(projectModel);
                    _db.DbProject.Insert(project);

                    TempData[OperationResult.Success.ToString()] = "Projeto salvo com sucesso!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData[OperationResult.Error.ToString()] = "Por favor preencha todos os campos obrigatórios!";
                    return View(projectModel);
                }
            }
            catch (Exception e)
            {
                Logger.SaveLog(e, _configuration);
                TempData[OperationResult.Error.ToString()] = "Ocorreu um erro ao salvar projeto!";
                return RedirectToAction("Index");
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
    }
}