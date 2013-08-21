using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoDoc.Models
{
    [Table("UserProfiles")]
    public partial class User
    {
        [Key, DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required, Display(Name = "Логин"), MaxLength(20)]
        public string UserLogin { get; set; }

        [Required, Display(Name = "Имя"), MaxLength(50)]
        public string FirstName { get; set; }

        [Required, Display(Name = "Отчество"), MaxLength(50)]
        public string MiddleName { get; set; }

        [Required, Display(Name = "Фамилия"), MaxLength(50)]
        public string LastName { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }

        [NotMapped, Required, Display(Name = "Пароль")]
        [StringLength(100, ErrorMessage = "Значение \"{0}\" должно содержать не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped, Display(Name = "Подтверждение пароля")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }

        [NotMapped, Display(Name = "Уровень Доступа")]
        public string Role { get; set; }

        [NotMapped]
        public ICollection<SelectListItem> PossibleRoles { get; set; }
    }

    public class LoginUser
    {
        [Key]
        public int UserId { get; set; }

        [Required, Display(Name = "Логин"), MaxLength(20)]
        public string UserLogin { get; set; }

        [Required, Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}