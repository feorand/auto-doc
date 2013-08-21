using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoDoc.Models
{
    [Table("Groups")]
    public class Group
    {
        [Key, DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, Display(Name="Название"), MaxLength(10)]
        public string GroupName { get; set; }

        [Display(Name="Студенты")]
        public virtual ICollection<Student> Students { get; set; }

        [Display(Name="Дисциплины")]
        public virtual ICollection<Subject> Subjects { get; set; }
    }

    [NotMapped]
    public class RatingGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RatingStudent> Students { get; set; }
        public ICollection<RatingSubject> Subjects { get; set; }
    }
}
