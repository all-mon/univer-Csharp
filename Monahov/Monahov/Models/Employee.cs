using System.ComponentModel.DataAnnotations.Schema;

namespace Monahov.Models
{
    public class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //имя
        public string FirstName { get; set; }
        //фамилия
        public string LastName { get; set; }
        //оклад
        public double Salary { get; set; }
        //премия
        public int Premium { get; set; }
        //должность
        public string Position { get; set; }
        //статус(уволен/работает)
        public bool IsDismissed { get; set; }
    }
}
