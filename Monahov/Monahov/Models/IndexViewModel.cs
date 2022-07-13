using System.Collections.Generic;

namespace Monahov.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }
        public SortViewModel SortViewModel { get; set; }
    }
}
