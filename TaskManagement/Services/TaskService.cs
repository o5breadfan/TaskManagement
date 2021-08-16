using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Models;

namespace TaskManagement.Services
{
    public class TaskService
    {
        private ApplicationContext _applicationContext;

        public TaskService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public SeedTasks GetListTasks()
        {
            SeedTasks model = new() { Seed = null, Tasks = _applicationContext.Tasks.ToList() };
            return model;
        }

        public bool CreateTask(DoTask model)
        {
            DoTask task = new DoTask { Name = model.Name, Description = model.Description, Executors = model.Executors, DateRegister = model.DateRegister, Status = model.Status };
            _applicationContext.Tasks.Add(task);
            _applicationContext.SaveChanges();
            return true;

        }

    }
}
