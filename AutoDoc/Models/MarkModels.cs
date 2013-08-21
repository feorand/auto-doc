using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoDoc.Models
{
    [Table("Marks")]
    public class Mark : IValidatableObject
    {
        private AutoDocContext _db = new AutoDocContext();

        [Key, DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name="Студент")]
        public int StudentId { get; set; }

        [Display(Name="Форма контроля")]
        public int FormId{ get; set; }

        [Display(Name="Баллы")]
        public int Score { get; set; }

        [NotMapped, Display(Name = "Студент")]
        public string StudentName { get; set; }

        [NotMapped, Display(Name = "Форма контроля")]
        public string FormName { get; set; }

        [NotMapped]
        public int SubjectId { get; set; }

        [NotMapped, Display(Name="Максимум баллов")]
        public int MaxScore { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var cform = _db.ControlForms.Find(FormId);
            if (cform.MaxScore < Score)
            {
                yield return new ValidationResult(String.Format("Максимальная оценка по данной форме контроля: {0}", cform.MaxScore.ToString()));            
            } 
            else
            {
                if (Score < 0)
                {
                    yield return new ValidationResult("Оценка не может быть отрицательной");
                }
                else
                {
                    yield return ValidationResult.Success;
                }
            }
        }
    }

    [NotMapped]
    public class RatingMark
    {
        public int SubjectId { get; set; }
        public int Score { get; set; }
        public int Rating { get; set; }
    }
}
