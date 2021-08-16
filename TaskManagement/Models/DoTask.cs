using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models
{
    public enum Status
    { 
        Assigned,
        InProgress,
        Suspended,
        Done
    }

    public class DoTask
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Executors { get; set; }
        public DateTime? DateRegister { get; set; }
        public Status? Status { get; set; }
        public double? PlanTime { get; set; }
        public double? FactTime { get; set; }
        public DateTime? DateFinished { get; set; }
        public int? ParentId { get; set; }

    }
}
