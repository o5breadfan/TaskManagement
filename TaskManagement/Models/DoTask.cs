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
        [Display(Name ="Назначена")]
        Assigned,
        [Display(Name ="Выполняется")]
        InProgress,
        [Display(Name ="Приостановлена")]
        Suspended,
        [Display(Name ="Завершена")]
        Done
    }

    public class DoTask
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(45)]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name ="Наименование задачи")]
        [Column(TypeName ="nvarchar(45)")]
        public string Name { get; set; }
        [MaxLength(100)]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name ="Описание задачи")]
        [Column(TypeName = "nvarchar(100)")]
        public string Description { get; set; }
        [MaxLength(45)]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name ="Исполнители")]
        [Column(TypeName = "nvarchar(45)")]
        public string Executors { get; set; }
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name ="Дата регистрации задачи")]
        public DateTime? DateRegister { get; set; }
        public Status? Status { get; set; }
        public double? PlanTime { get; set; }
        public double? FactTime { get; set; }
        public DateTime? DateFinished { get; set; }
        public int? ParentId { get; set; }

    }
}
