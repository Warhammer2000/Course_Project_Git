using CourseProjectItems.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CourseProjectItems.Interfaces
{
    public interface ICollectionService
    {
        Task<IEnumerable<Collection>> GetAllCollections();
        Task<Collection> GetCollectionById(int id);
        Task AddCollection(Collection collection);
        Task UpdateCollection(Collection collection);
        Task DeleteCollection(int id);
        Task<IEnumerable<Collection>> GetCollectionsByUserId(string userId);
    }
}
