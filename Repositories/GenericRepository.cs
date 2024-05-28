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
			return await _dbSet.ToListAsync(); // Получить все записи
		}

		public async Task<T> GetById(int id)
		{
			return await _dbSet.FindAsync(id); // Найти запись по ID
		}

		public async Task Add(T entity)
		{
			await _dbSet.AddAsync(entity); // Добавить новую запись
			await _context.SaveChangesAsync(); // Сохранить изменения
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
			var entity = await GetById(id); // Найти запись по ID
			if (entity != null)
			{
				_dbSet.Remove(entity); // Удалить запись
				await _context.SaveChangesAsync(); // Сохранить изменения
			}
		}

		public IQueryable<T> Find(Expression<Func<T, bool>> expression)
		{
			return _context.Set<T>().Where(expression);
		}
	}
}
