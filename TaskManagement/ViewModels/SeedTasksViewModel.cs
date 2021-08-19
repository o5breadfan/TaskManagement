using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Models;

namespace TaskManagement.ViewModels
{
    public class SeedTasksViewModel
    {
        public int? Seed { get; set; }
        public IList<DoTask> Tasks { get; set; }
    }
}
