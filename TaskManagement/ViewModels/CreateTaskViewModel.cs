using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagement.ViewModels
{
    public class CreateTaskViewModel
    {
        private DateTime _localDate = DateTime.Now;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Executors { get; set; }
        public DateTime DateRegister { get { return _localDate; } set { _localDate = value; } }
        public double PlanTime { get; set; }
        public int? ParentId { get; set; }
    }
}
