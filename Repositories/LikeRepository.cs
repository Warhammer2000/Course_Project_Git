using CourseProjectItems.Data;
using CourseProjectItems.Interfaces;
using CourseProjectItems.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProjectItems.Repositories
{
	public class LikeRepository : GenericRepository<Like>, ILikeRepository
	{
		public LikeRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<bool> IsLikedByUser(int itemId, string userId)
		{
			return await _context.Likes.AnyAsync(l => l.ItemId == itemId && l.UserId == userId);
		}
	}
}
