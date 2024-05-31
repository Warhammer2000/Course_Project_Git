using CourseProjectItems.Models;

namespace CourseProjectItems.Interfaces;

public interface ICollectionRepository : IGenericRepository<Collection>
{
	Task<IEnumerable<Collection>> GetCollectionsByUserId(string userId);
}