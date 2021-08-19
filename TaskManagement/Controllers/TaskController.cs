using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Models;
using TaskManagement.Services;
using TaskManagement.ViewModels;

namespace TaskManagement.Controllers
{
    public class TaskController : Controller
    {
        private TaskService _taskService;
        private StatusService _statusService;
        public TaskController(TaskService taskService, StatusService statusService)
        {
            _taskService = taskService;
            _statusService = statusService;
        }

        public IActionResult Index()
        {
            return View(_taskService.GetListTasks());
        }

        public IActionResult CreateTask()
        {
            return View(new DoTask());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTask(DoTask task)
        {
            if (ModelState.IsValid)
            {
                _taskService.CreateTask(task);
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_TreeViewPartial", _taskService.GetListTasks()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "CreateTask",task) });
        }

        [HttpGet]
        public IActionResult EditTask(int id)
        {
            var taskModel = _taskService.GetTask(id);
            if (taskModel == null)
                return NotFound();
            return View(taskModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTask(DoTask task, int id)
        {
            if (ModelState.IsValid)
            {
                _taskService.UpdateTask(task);
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_TreeViewPartial", _taskService.GetListTasks()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "EditTask",task) });

        }

        public IActionResult Details(int id)
        {
            var task = _taskService.GetTask(id);
            DetailViewModel model = new() { Id = task.Id, Name = task.Name, Description = task.Description, Executors = task.Executors, PlanTime = task.PlanTime, Status = task.Status, SubTasks = _taskService.GetSubTasksList(id) };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTask(int? id)
        {
            if(_taskService.DeleteTask(id))
                return Json(new { html = Helper.RenderRazorViewToString(this, "_TreeViewPartial", _taskService.GetListTasks()) });
            return NotFound();
        }

        [HttpGet]
        public IActionResult CreateSubTask(int id)
        {
            var model = new DoTask() { ParentId = id };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateSubTask(DoTask model, int id)
        {
            if(ModelState.IsValid)
            {
                if (_taskService.CreateSubTask(model, id))
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_TreeViewPartial", _taskService.GetListTasks()) });
                else
                    return NotFound();

            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "CreateOrEditTask", model) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeStatusAssign(int id)
        {
            _statusService.UpdateStatus(Status.InProgress, id);
            return Json(new { html = Helper.RenderRazorViewToString(this, "_TreeViewPartial", _taskService.GetListTasks()) }); ;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeStatusProgress(string button, int id)
        {
            switch (button)
            {
                case "done":
                    _statusService.UpdateStatus(Status.Done, id);
                    _taskService.CompleteTask(id);
                    break;
                case "pause":
                    _statusService.UpdateStatus(Status.Suspended, id);
                    break;
            }
            return Json(new { html = Helper.RenderRazorViewToString(this, "_TreeViewPartial", _taskService.GetListTasks()) }); ;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeStatusSuspended(int id)
        {
            _statusService.UpdateStatus(Status.InProgress, id);
            return Json(new { html = Helper.RenderRazorViewToString(this, "_TreeViewPartial", _taskService.GetListTasks()) }); ;
        }
    }
}
