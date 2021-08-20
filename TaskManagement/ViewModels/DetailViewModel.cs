using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace TaskManagement.ViewModels
{
    public class DetailViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Наименование задачи")]
        public string Name { get; set; }
        [Display(Name ="Описание задачи")]
        public string Description { get; set; }
        [Display(Name ="Исполнители")]
        public string Executors { get; set; }
        [Display(Name = "Плановое время выполнения задачи (в часах)")]
        public double PlanTime { get; set; }
        [Display(Name ="Текущий статус задачи")]
        public Status Status { get; set; }
        public int? ParentId { get; set; }
        public IList<DoTask> SubTasks { get; set; }
    }
}
