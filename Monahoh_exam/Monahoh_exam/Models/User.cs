using Microsoft.AspNetCore.Identity;

namespace Monahoh_exam.Models
{
    public class User: IdentityUser
    {
        public int Year { get; set; }
    }
}
