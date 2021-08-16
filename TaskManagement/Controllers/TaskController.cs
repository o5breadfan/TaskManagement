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
            return View(_taskService.GetListTasks());
        }

        public IActionResult CreateOrEditTask()
        {
            return View(new DoTask());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEditTask(DoTask task)
        {
            if (ModelState.IsValid)
            {
                _taskService.CreateTask(task);
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_TreeViewPartial", _taskService.GetListTasks()) });

            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "CreateOrEditTask",task) });
        }


    }
}
