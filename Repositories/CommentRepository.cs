using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseProjectItems.Data;
using CourseProjectItems.Interfaces;
using CourseProjectItems.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProjectItems.Repositories
{
	public class CommentRepository : GenericRepository<Comment>, ICommentRepository
	{
		public CommentRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<IEnumerable<Comment>> GetCommentsByItemId(int itemId)
		{
			return await _context.Comments.Where(c => c.ItemId == itemId).ToListAsync();
		}
	}
}
