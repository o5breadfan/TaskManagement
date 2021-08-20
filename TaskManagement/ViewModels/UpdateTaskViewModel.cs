using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Models;

namespace TaskManagement.ViewModels
{
    public class UpdateTaskViewModel
    {
        public int Id { get; set; }

        [MaxLength(45)]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Наименование задачи")]
        public string Name { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Описание задачи")]
        public string Description { get; set; }

        [MaxLength(45)]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Исполнители")]
        public string Executors { get; set; }

        [Display(Name = "Плановое время выполнения задачи (в часах)")]
        public double PlanTime { get; set; }

        public Status Status { get; set; }
    }
}
