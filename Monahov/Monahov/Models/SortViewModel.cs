namespace Monahov.Models
{
    public class SortViewModel
    {
        public SortEmployee NameSort { get; set; }
        public SortEmployee PositionSort { get; set; }
        public SortEmployee IsDismissedSort { get; set; }
        public SortEmployee Current { get; set; }     // значение свойства, выбранного для сортировки
        public bool Up { get; set; } //сортировка по возврастанию или убыванию

        public SortViewModel(SortEmployee sortOrder)
        {
            // значения по умолчанию
            NameSort = SortEmployee.NameAsc;
            PositionSort = SortEmployee.PositionAsc;
            IsDismissedSort = SortEmployee.IsDismissedAsc;
            Up = true;

            if (sortOrder == SortEmployee.NameDesc || sortOrder == SortEmployee.PositionDesc || sortOrder == SortEmployee.IsDismissedDesc)
            {
                Up = false;
            }

            switch (sortOrder)
            {
                
                case SortEmployee.NameDesc:
                    Current = NameSort = SortEmployee.NameAsc;
                    break;
                case SortEmployee.PositionAsc:
                    Current = PositionSort = SortEmployee.PositionDesc;
                    break;
                case SortEmployee.PositionDesc:
                    Current = PositionSort = SortEmployee.PositionAsc;
                    break;
                case SortEmployee.IsDismissedAsc:
                    Current = IsDismissedSort = SortEmployee.IsDismissedDesc;
                    break;
                case SortEmployee.IsDismissedDesc:
                    Current = IsDismissedSort = SortEmployee.IsDismissedAsc;
                    break;
                default:
                    Current = NameSort = SortEmployee.NameDesc;
                    break;
            }
        }
    }

}
