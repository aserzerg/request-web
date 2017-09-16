using System.ComponentModel.DataAnnotations;

namespace request_web.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Необходимо заполнить поле {0}.")]
        public string UserName { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Необходимо заполнить поле {0}.")]
        public string Password { get; set; }

    }
}