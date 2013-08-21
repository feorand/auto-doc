using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoDoc.Models
{
    [Table("Subjects")]
    public class Subject
    {
        [Key, DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, Display(Name = "Название"), MaxLength(50)]
        public string SubjectName { get; set; }

        [Required, Display(Name = "Аббревиатура"), MaxLength(6)]
        public string ShortName { get; set; }

        [Display(Name = "Группа")]
        public int GroupId { get; set; }

        [Display(Name = "Разделы")]
        public virtual ICollection<Section> Sections { get; set; }

        [Display(Name="Преподаватели")]
        public virtual ICollection<User> Lectors { get; set; }

        //public virtual User User { get; set; }
    }

    [NotMapped]
    public class DisplaySubject
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public string GroupName { get; set; }
        public int MaxScore { get; set; }
        public int AllowedScore { get; set; }
        public ICollection<DisplayStudent> Students { get; set; }
        public ICollection<DisplaySection> Sections { get; set; }
        public ICollection<User> Lectors { get; set; }
    }

    [NotMapped]
    public class RatingSubject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public double AverageScore { get; set; }
    }

    [NotMapped]
    public class GrantSubject
    {
        public Subject Subject {get; set;}
        public ICollection<User> Lectors { get; set; }
    }
}