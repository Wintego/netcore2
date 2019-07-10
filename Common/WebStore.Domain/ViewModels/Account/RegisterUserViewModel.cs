using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain.ViewModels.Account
{
    public class RegisterUserViewModel
    {
        [Display(Name = "Имя пользователя"), MaxLength(256, ErrorMessage = "Максимальная длина 256 символов")]
        public string UserName { get; set; }

        [Display(Name = "Пароль"), Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Подтвердить пароль"), Required, DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Пароли не совпадают"),]
        public string ConfirmPassword { get; set; }
    }
}