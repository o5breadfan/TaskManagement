using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private TaskService _taskService;

        public HomeController(ILogger<HomeController> logger,TaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }

        
        public IActionResult Index()
        {
            return View(_taskService.GetListTasks());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
