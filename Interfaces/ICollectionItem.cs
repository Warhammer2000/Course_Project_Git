using CourseProjectItems.Models.Enums;

namespace CourseProjectItems.Interfaces
{
    public interface ICollectionItem
    {
        string Name { get; set; }
        CollectionType Type { get; set; }
    }
}
