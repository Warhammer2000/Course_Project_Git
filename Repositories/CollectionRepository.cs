using CourseProjectItems.Data;
using CourseProjectItems.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using CourseProjectItems.Interfaces;


namespace CourseProjectItems.Repositories
{
	public class CollectionRepository : GenericRepository<Collection>, ICollectionRepository
	{
		public CollectionRepository(ApplicationDbContext context) : base(context) { }

		public async Task<IEnumerable<Collection>> GetCollectionsByUserId(string userId)
		{
			return await _context.Collections.Where(c => c.AuthorID == userId).ToListAsync(); // Изменено на AuthorID
		}
	}
}
