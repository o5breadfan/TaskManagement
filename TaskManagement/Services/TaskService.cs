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

        public bool CreateTask(DoTask model)
        {
            DoTask task = new DoTask { Name = model.Name, Description = model.Description, Executors = model.Executors, DateRegister = model.DateRegister, Status = Status.Assigned, PlanTime = model.PlanTime };
            _applicationContext.Add(task);
            _applicationContext.SaveChanges();
            return true;
        }

        public bool CreateSubTask(DoTask model, int id)
        {
            var parentTask = GetTask(id);
            DoTask subTask = new() { Name = model.Name, Description = model.Description, Executors = model.Executors, DateRegister = model.DateRegister, Status = Status.Assigned, PlanTime = model.PlanTime, ParentId = id };
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

        public bool UpdateTask(DoTask model)
        {
            _applicationContext.Update(model);
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
