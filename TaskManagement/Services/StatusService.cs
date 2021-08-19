using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Models;

namespace TaskManagement.Services
{
    public class StatusService
    {
        private ApplicationContext _applicationContext;
        private TaskService _taskService;

        public StatusService(ApplicationContext applicationContext, TaskService taskService)
        {
            _applicationContext = applicationContext;
            _taskService = taskService;
        }
        public void UpdateStatus(Status status, int id)
        {
            var task = _taskService.GetTask(id);
            task.Status = status;
            _applicationContext.Update(task);
            _applicationContext.SaveChanges();
        }
    }
}
