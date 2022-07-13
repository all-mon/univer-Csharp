using System.ComponentModel.DataAnnotations;

namespace CP_AuthCookieApp.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Не указан Email")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
