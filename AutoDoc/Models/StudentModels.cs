using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoDoc.Models
{
    [Table("Students")]
    public class Student
    {
        [Key, DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, Display(Name="Фамилия"), MaxLength(50)]
        public string LastName { get; set; }

        [Required, Display(Name="Имя"), MaxLength(50)]
        public string FirstName { get; set; }

        [Required, Display(Name="Отчество"), MaxLength(50)]
        public string MiddleName { get; set; }

        public int GroupId { get; set; }

        [NotMapped]
        public string GroupName { get; set; }

        public virtual ICollection<Mark> Marks { get; set; }
    }

    [NotMapped]
    public class DisplayStudent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public int TotalScore { get; set; }
        public bool IsAllowed { get; set; }
        public ICollection<DisplaySectionStudent> Sections { get; set; }
    }

    [NotMapped]
    public class RatingStudent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public ICollection<RatingMark> Marks { get; set; }
        public double Score { get; set; }
        public int Rating { get; set; }
    }
}