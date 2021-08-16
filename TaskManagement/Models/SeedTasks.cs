using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagement.Models
{
    public class SeedTasks
    {
        public int? Seed { get; set; }
        public IList<DoTask> Tasks { get; set; }
    }
}
