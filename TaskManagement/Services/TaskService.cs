using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Models;
using TaskManagement.ViewModels;

namespace TaskManagement.Services
{
    public class TaskService
    {
        private ApplicationContext _applicationContext;
        private StatusService _statusService;

        public TaskService(ApplicationContext applicationContext, StatusService statusService)
        {
            _applicationContext = applicationContext;
            _statusService = statusService;
        }

        public SeedTasksViewModel GetListTasks()
        {
            SeedTasksViewModel model = new SeedTasksViewModel() { Seed = null, Tasks = _applicationContext.Tasks.ToList() };
            return model;
        }

        public bool CreateTask(CreateTaskViewModel model)
        {
            DoTask task = new DoTask { Name = model.Name, Description = model.Description, Executors = model.Executors, Status = Status.Assigned, PlanTime = model.PlanTime };
            _applicationContext.Add(task);
            _applicationContext.SaveChanges();
            return true;
        }

        public bool CreateSubTask(CreateTaskViewModel model)
        {
            var id = model.ParentId;
            var parentTask = GetTask(id);
            DoTask subTask = new() { Name = model.Name, Description = model.Description, Executors = model.Executors, Status = Status.Assigned, PlanTime = model.PlanTime, ParentId = id };
            if (parentTask.PlanTime < GetSumPlanTime(id)+model.PlanTime)
            {
                parentTask.PlanTime += (GetSumPlanTime(id) + model.PlanTime - parentTask.PlanTime);
                _applicationContext.Update(parentTask);
                _applicationContext.Add(subTask);
                _applicationContext.SaveChanges();
            }
            else
            {
                _applicationContext.Add(subTask);
                _applicationContext.SaveChanges();
            }

            return true;
        }

        public bool UpdateTask(UpdateTaskViewModel viewModel)
        {
            DoTask task = _applicationContext.Tasks.Find(viewModel.Id);
            task.Name = viewModel.Name;
            task.PlanTime = viewModel.PlanTime;
            task.Executors = viewModel.Executors;
            task.Description = viewModel.Description;
            _applicationContext.SaveChanges();
            return true;
        }

        public bool DeleteTask(int? id)
        {
            var currentNode = GetTask(id);
            List<DoTask> list = new() { currentNode };
            while ((currentNode = _applicationContext.Tasks.FirstOrDefault(i => i.ParentId == currentNode.Id)) != null)
                list.Add(currentNode);
            if (list != null)
            {
                _applicationContext.RemoveRange(list);
                _applicationContext.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public DoTask GetTask(int? id)
        {
            return _applicationContext.Tasks.Find(id);
        }

        public UpdateTaskViewModel GetUpdateTask(int? id)
        {
            DoTask doTask =_applicationContext.Tasks.Find(id);
            UpdateTaskViewModel task = new()
            {
                Id = doTask.Id,
                Name = doTask.Name,
                Executors = doTask.Executors,
                Description = doTask.Description,
                PlanTime = doTask.PlanTime,
                Status = doTask.Status
            };
            return task;
        }

        public double GetSumPlanTime(int? idParent)
        {
            double sum = 0;
            List<DoTask> subTasks = _applicationContext.Tasks.Where(x => x.ParentId == idParent).ToList();
            foreach (var task in subTasks)
                sum += task.PlanTime;
            return sum;
        }

        public void CompleteTask(int? id)
        {
            var task = GetTask(id);
            task.DateFinished = DateTime.Now;
            task.FactTime = CalcFactTime(task.DateRegister, DateTime.Now);
            _applicationContext.Update(task);
            if (task.ParentId != null && _statusService.CheckCompletionStatusSubTasks(task.ParentId))
            {
                _statusService.UpdateStatus(Status.Done, task.ParentId);
                CompleteTask(task.ParentId);
            }
            _applicationContext.SaveChanges();

        }

        public double CalcFactTime(DateTime dateRegister, DateTime dateFinished)
        {
            return Math.Round(dateFinished.Subtract(dateRegister).TotalHours);
        }

        public List<DoTask> GetSubTasksList(int? parentId)
        {
            return _applicationContext.Tasks.Where(x => x.ParentId == parentId).ToList();
        }
     
    }
}
