using CourseProjectItems.Interfaces;
using CourseProjectItems.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CourseProjectItems.ViewModels
{
    public class ViewModelOptions<T> where T : ICollectionItem
    {
        public IQueryable<T> Collections { get; set; }

        public (IQueryable<T> Result, int PageSizeCollection) GetSortedAndFilteredCollection(
            CollectionType? type, int page, SortState sortOrder)
        {
            var query = Collections;
            if (type.HasValue)
            {
                query = query.Where(c => c.Type == type.Value);
            }

            switch (sortOrder)
            {
                case SortState.NameDesc:
                    query = query.OrderByDescending(c => c.Name);
                    break;
                case SortState.NameAsc:
                default:
                    query = query.OrderBy(c => c.Name);
                    break;
            }

            int pageSize = 10;
            var result = query.Skip((page - 1) * pageSize).Take(pageSize);
            return (result, pageSize);
        }
    }
}
