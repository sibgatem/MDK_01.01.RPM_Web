using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebASP.ViewModels
{
    public class RegisterViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Login { get; set; }
        [Required, RegularExpression("^[а-яА-Я]*$", ErrorMessage = "Фамилия может содержать только латинские буквы!")]
        public string Surname { get; set; }
        [Required, RegularExpression("^[а-яА-Я]*$", ErrorMessage = "Имя может содержать только латинские буквы!")]
        public string Name { get; set; }
        [RegularExpression("^[а-яА-Я]*$", ErrorMessage = "Отчество может содержать только латинские буквы!")]
        public string middle_Name { get; set; }
        [Required]
        public int Role_Id { get; set; }
        [Required]
        public DateTime birth_day { get; set; }

        [DataType(DataType.Password)]
        public virtual string Password { get; set; }

        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public virtual string ConfirmPassword { get; set; }
    }
}
