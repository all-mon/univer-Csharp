using System.Collections.Generic;

namespace CP_AuthCookieApp.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //Данные классы связаны отношением один-ко-многим,
        //то есть один пользователь может иметь только одну роль,
        //а к одной роли могут принадлежать несколько пользователей.
        public List<User> Users { get; set; }
        public Role()
        {
            Users = new List<User>();
        }
    }
}
