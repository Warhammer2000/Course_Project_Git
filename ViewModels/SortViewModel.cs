using CourseProjectItems.Models.Enums;

namespace CourseProjectItems.ViewModels
{
    public class SortViewModel
    {
        public SortState Current { get; set; }

        public SortViewModel(SortState sortOrder)
        {
            Current = sortOrder;
        }
    }
}
