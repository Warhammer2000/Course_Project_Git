using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using CourseProjectItems.Data;
using CourseProjectItems.Interfaces;
using CourseProjectItems.Entity;

namespace CourseProjectItems.Repositories
{	
	public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		protected readonly ApplicationDbContext _context; 
		private readonly DbSet<T> _dbSet; 

		public GenericRepository(ApplicationDbContext context)
		{
			_context = context;
			_dbSet = _context.Set<T>();
		}

		public async Task<IEnumerable<T>> GetAll()
		{
			return await _dbSet.ToListAsync(); 
		}

		public async Task<T> GetById(int id)
		{
			return await _dbSet.FindAsync(id); 
		}

		public async Task Add(T entity)
		{
			await _dbSet.AddAsync(entity); 
			await _context.SaveChangesAsync(); 
		}

		public async Task Update(T entity)
		{
            var existingEntity = await _dbSet.FindAsync(entity.Id);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached;
            }

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

		public async Task Delete(int id)
		{
			var entity = await GetById(id); 
			if (entity != null)
			{
				_dbSet.Remove(entity); 
				await _context.SaveChangesAsync(); 
			}
		}

		public IQueryable<T> Find(Expression<Func<T, bool>> expression)
		{
			return _context.Set<T>().Where(expression);
		}
	}
}
