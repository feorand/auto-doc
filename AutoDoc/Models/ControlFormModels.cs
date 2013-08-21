using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoDoc.Models
{
    [Table("ControlForms")]
    public class ControlForm : IValidatableObject
    {
        private AutoDocContext _db = new AutoDocContext();

        [Key, DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, Display(Name = "Краткое название"), MaxLength(6)]
        public string Name { get; set; }

        [Required, Display(Name = "Расшифровка"), MaxLength(50)]
        public string Description { get; set; }

        [Display(Name="Неделя"), Range(1, 17)]
        public int Week { get; set; }

        [Display(Name="Макс. баллов"), Range(1, 100)]
        public int MaxScore {get; set;}

        [Display(Name = "Раздел")]
        public int SectionId { get; set; }

        public virtual ICollection<Mark> Marks { get; set; }

        [NotMapped]
        public int SubjectId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var marks = _db.Marks.Where(x => x.FormId == Id).ToList();
            foreach (var item in marks)
            {
                if (item.Score > MaxScore)
                {
                    var student = _db.Students.Find(item.StudentId);
                    var name = string.Format("{0} {1}.{2}.", student.LastName, student.FirstName[0], student.MiddleName[0]);
                    yield return new ValidationResult(String.Format("Студент {0} уже имеет {1} баллов (это больше. чем {2})", name, item.Score, MaxScore));
                }
                else 
                {
                    yield return ValidationResult.Success;
                }
            }
        }
    }
}