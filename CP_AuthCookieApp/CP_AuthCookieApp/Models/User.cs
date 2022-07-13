using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CP_AuthCookieApp.Models
{
    //класс представляющий модель пользователя
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? RoleId { get; set; }
        public Role Role { get; set; }
        //следующая аннотация позваляет сохранять только дату(без времени) в БД
        [Column(TypeName = "date")]
        public DateTime DateOfRegistration { get; set; }//дата регистрации(или создания) пользователя
    }

}
