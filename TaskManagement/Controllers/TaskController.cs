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


        public IActionResult CreateOrEditTask(int id = 0)
        {
            if (id == 0) 
                return View(new DoTask());
            else
            {
                var taskModel = _taskService.GetTask(id);
                if (taskModel == null)
                    return NotFound();
                return View(taskModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEditTask(int id, DoTask task)
        {
            if (ModelState.IsValid)
            {
                if(id == 0)
                {
                    _taskService.CreateTask(task);

                }
                else
                {
                    try
                    {
                        _taskService.UpdateTask(task);
                    }
                    catch(Exception)
                    {
                        throw;
                    }
                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_TreeViewPartial", _taskService.GetListTasks()) });

            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "CreateOrEditTask",task) });
        }

        public IActionResult Details(int id)
        {
            var model = _taskService.GetTask(id);
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
                if (_taskService.CreateSubTask(model,id))
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_TreeViewPartial", _taskService.GetListTasks()) });
                else
                    return NotFound();

            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "CreateOrEditTask", model) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeStatus(Status status, int id)
        {
            switch (status)
            {
                case Status.Assigned:
                    _statusService.UpdateStatus(Status.InProgress,id);
                    break;
                case Status.InProgress:
                    _statusService.UpdateStatus(Status.Done, id);
                    _taskService.CompleteTask(id);
                    break;
                case Status.Suspended:
                    _statusService.UpdateStatus(Status.InProgress, id);
                    break;
            }
            return Json(new { html = Helper.RenderRazorViewToString(this, "_TreeViewPartial", _taskService.GetListTasks()) }); ;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PauseStatus(Status status, int id)
        {
            switch (status)
            {
                case Status.Assigned:
                    _statusService.UpdateStatus(status, id);
                    break;
                case Status.InProgress:
                    _statusService.UpdateStatus(Status.Suspended, id);
                    break;
            }
            return Json(new { html = Helper.RenderRazorViewToString(this, "_TreeViewPartial", _taskService.GetListTasks()) });
        }
    }
}
