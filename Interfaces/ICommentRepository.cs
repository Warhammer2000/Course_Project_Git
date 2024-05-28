using System.Collections.Generic;
using System.Threading.Tasks;
using CourseProjectItems.Models;

namespace CourseProjectItems.Interfaces
{
	public interface ICommentRepository : IGenericRepository<Comment>
	{
		Task<IEnumerable<Comment>> GetCommentsByItemId(int itemId);
	}
}
