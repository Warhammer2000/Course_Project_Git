using System.Threading.Tasks;
using System.Collections.Generic;
using CourseProjectItems.Models;
using CourseProjectItems.Interfaces;

namespace CourseProjectItems.Services
{
    public class CollectionService : ICollectionService
	{
		private readonly ICollectionRepository _collectionRepository;

		public CollectionService(ICollectionRepository collectionRepository)
		{
			_collectionRepository = collectionRepository;
		}

		public async Task<IEnumerable<Collection>> GetAllCollections()
		{
			return await _collectionRepository.GetAll();
		}

		public async Task<Collection> GetCollectionById(int id)
		{
			return await _collectionRepository.GetById(id);
		}

		public async Task AddCollection(Collection collection)
		{
			await _collectionRepository.Add(collection);
		}

		public async Task UpdateCollection(Collection collection)
		{
			await _collectionRepository.Update(collection);
		}

		public async Task DeleteCollection(int id)
		{
			await _collectionRepository.Delete(id);
		}

		public async Task<IEnumerable<Collection>> GetCollectionsByUserId(string userId)
		{
			return await _collectionRepository.GetCollectionsByUserId(userId);
		}
	}

}
