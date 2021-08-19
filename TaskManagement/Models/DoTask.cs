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
        private DateTime _localDate = DateTime.Now;
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
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Поле обязательно для заполнения"), DataType(DataType.DateTime)]
        [Display(Name ="Дата регистрации задачи")]
        public DateTime DateRegister { get { return _localDate; } set { _localDate = value; } }
        [Display(Name ="Текущий статус задачи")]
        public Status Status { get; set; }
        [Display(Name = "Плановое время выполнения задачи (в часах)")]
        public double PlanTime { get; set; }
        public double? FactTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? DateFinished { get; set; }
        public int? ParentId { get; set; }

    }
}
