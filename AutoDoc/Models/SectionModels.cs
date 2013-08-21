using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoDoc.Models
{
    [Table("Sections")]
    public class Section
    {
        [Key, DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, Display(Name="Название"), MaxLength(10)]
        public string Name { get; set; }

        public int SubjectId { get; set; }

        [Display(Name="Формы контроля")]
        public virtual ICollection<ControlForm> ControlForms { get; set; }

        [NotMapped, Display(Name="Сумма баллов")]
        public int Sum { get; set; }
    }

    [NotMapped]
    public class DisplaySection
    {
        public int id { get; set; }
        public string Name { get; set; }
        public int MaxScore { get; set; }
        public int AllowedScore { get; set; }
        public ICollection<ControlForm> ControlForms { get; set; }
    }

    [NotMapped]
    public class DisplaySectionStudent
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int TotalScore { get; set; }
        public ICollection<Mark> Marks { get; set; }
    }
}
