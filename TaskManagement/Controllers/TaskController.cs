using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.Controllers
{
    public class TaskController : Controller
    {
        private TaskService _taskService;
        public TaskController(TaskService taskService)
        {
            _taskService = taskService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            DoTask task = new DoTask();
            return PartialView("CreateTaskPartial", task);
        }

        [HttpPost]
        public IActionResult Create(DoTask task)
        {
            if (ModelState.IsValid)
            {
                if(_taskService.CreateTask(task))
                    return View("Index");
            }
            return View("Index");
        }


    }
}
