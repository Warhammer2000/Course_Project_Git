using CourseProjectItems.Models;
using CourseProjectItems.Models.Enums;

namespace CourseProjectItems.ViewModels
{
    public class FilterViewModel
    {
        public IEnumerable<Collection> Collections { get; set; }
        public CollectionType? SelectedType { get; set; }
        public string Type { get; set; }

        public FilterViewModel(IEnumerable<Collection> collections, CollectionType? selectedType, string type)
        {
            Collections = collections;
            SelectedType = selectedType;
            Type = type;
        }
    }
}
