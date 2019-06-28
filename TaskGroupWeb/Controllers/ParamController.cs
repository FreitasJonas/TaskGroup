using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskGroupWeb.Controllers
{
    [Authorize]
    public class ParamController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}