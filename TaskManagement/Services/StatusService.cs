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

        public StatusService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        public void UpdateStatus(Status status, int? id)
        {
            var task = _applicationContext.Tasks.Find(id);
            task.Status = status;
            _applicationContext.Update(task);
            _applicationContext.SaveChanges();
        }

        // проверка всех подзадач на завершенность
        public bool CheckCompletionStatusSubTasks(int? parentId)
        {
            if (_applicationContext.Tasks.Where(x => x.ParentId == parentId).All(x => x.Status == Status.Done))
                return true;
            return false;
        }
    }
}
