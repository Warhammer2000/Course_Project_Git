using System.Linq.Expressions;

namespace CourseProjectItems.Interfaces
{
	public interface IGenericRepository<T> where T : class
	{
		Task<IEnumerable<T>> GetAll(); 
		Task<T> GetById(int id); 
		Task Add(T entity);
		Task Update(T entity); 
		Task Delete(int id); 
		IQueryable<T> Find(Expression<Func<T, bool>> expression);
	}
}
