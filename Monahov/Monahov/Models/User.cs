using Microsoft.AspNetCore.Identity;

namespace Monahov.Models
{
    public class User:IdentityUser
    {
        //год рождения
        public int Year { get; set; }
        //должность
        public string Position { get; set; }
    }
}
