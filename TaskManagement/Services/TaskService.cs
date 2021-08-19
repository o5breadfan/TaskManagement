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

        public TaskService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
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

        public bool CreateSubTask(DoTask model,int id)
        {
            var parentTask = GetTask(model.ParentId);
            //var parentTask1 = model.ParentId;
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
            List<DoTask> task = _applicationContext.Tasks.Where(x => (x.Id == id || x.ParentId == id)).ToList();
            if (task != null)
            {
                _applicationContext.RemoveRange(task);
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

        public double GetSumPlanTime(int idParent)
        {
            double sum = 0;
            List<DoTask> subTasks = _applicationContext.Tasks.Where(x => x.ParentId == idParent).ToList();
            foreach (var task in subTasks)
                sum += task.PlanTime;
            return sum;
        }

        public void CompleteTask(int id)
        {
            var task = GetTask(id);
            task.DateFinished = DateTime.Now;
            task.FactTime = CalcFactTime(task.DateRegister, DateTime.Now);
            _applicationContext.Update(task);
            _applicationContext.SaveChanges();

        }

        public double CalcFactTime(DateTime dateRegister, DateTime dateFinished)
        {
            return Math.Round(dateFinished.Subtract(dateRegister).TotalHours);
        }
     
    }
}
