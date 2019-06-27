using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class Employee
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Display(Name="Имя"), Required(ErrorMessage = "Имя является обязательным")]
        [RegularExpression(@"^([А-Я][а-я]{2,20})$", ErrorMessage = "Неверный формат имени")]
        public string FirstName { get; set; }
        [RegularExpression(@"^([А-Я][а-я]{2,20})$", ErrorMessage = "Неверный формат фамилии")]
        [Display(Name="Фамилия"), Required(ErrorMessage = "Фамилия является обязательным")]
        public string SurName { get; set; }
        [RegularExpression(@"^([А-Я][а-я]{2,20})$", ErrorMessage = "Неверный формат отчества")]
        [Display(Name="Отчество")]
        public string Patronymic { get; set; }
        [Display(Name="Возраст")]
        [Range(18,50, ErrorMessage = "Возраст должен быть от 18 до 50")]
        public int Age { get; set; }
    }
}