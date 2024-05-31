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
            var collections = await _collectionRepository.GetAll();
            if (collections == null)
            {
                throw new InvalidOperationException("No collections found");
            }
            return await _collectionRepository.GetAll();
		}

		public async Task<Collection> GetCollectionById(int id)
		{
            if (id <= 0)
            {
                throw new ArgumentException("Invalid collection ID");
            }

            var collection = await _collectionRepository.GetById(id);
            if (collection == null)
            {
                throw new KeyNotFoundException($"Collection with ID {id} not found");
            }
            return await _collectionRepository.GetById(id);
		}

		public async Task AddCollection(Collection collection)
		{
            if (string.IsNullOrWhiteSpace(collection.Name))
            {
                throw new ArgumentException("Collection name cannot be empty");
            }
			
            await _collectionRepository.Add(collection);
		}

		public async Task UpdateCollection(Collection collection)
		{
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (collection.Id <= 0)
            {
                throw new ArgumentException("Invalid collection ID");
            }

            var existingCollection = await _collectionRepository.GetById(collection.Id);
            if (existingCollection == null)
            {
                throw new KeyNotFoundException($"Collection with ID {collection.Id} not found");
            }
            await _collectionRepository.Update(collection);
		}

		public async Task DeleteCollection(int id)
		{
            if (id <= 0)
            {
                throw new ArgumentException("Invalid collection ID");
            }

            var existingCollection = await _collectionRepository.GetById(id);
            if (existingCollection == null)
            {
                throw new KeyNotFoundException($"Collection with ID {id} not found");
            }

            await _collectionRepository.Delete(id);
		}

		public async Task<IEnumerable<Collection>> GetCollectionsByUserId(string userId)
		{
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be empty");
            }

            var collections = await _collectionRepository.GetCollectionsByUserId(userId);
            if (collections == null)
            {
                throw new InvalidOperationException($"No collections found for user ID {userId}");
            }
            return await _collectionRepository.GetCollectionsByUserId(userId);
		}
	}

}
