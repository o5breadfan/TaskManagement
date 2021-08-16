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

        public List<DoTask> GetListTasks()
        {
            var tasks = _applicationContext.Tasks;
            SeedTasks model = new SeedTasks { Seed = null, Tasks = tasks.ToList() };
            return tasks.ToList();
        }

        public bool CreateTask(DoTask model)
        {
            DoTask task = new DoTask { Name = model.Name, Description = model.Description };
            _applicationContext.Tasks.Add(task);
            _applicationContext.SaveChanges();
            return true;

        }

    }
}
