using CourseProjectItems.Models;

namespace CourseProjectItems.Interfaces
{
	public interface ITicketsRepository
	{
        Task<IEnumerable<Ticket>> GetTicketsByUserIdAsync(string userId);
        Task AddAsync(Ticket ticket);
        Task<Ticket> GetByIdAsync(int id);
        Task UpdateAsync(Ticket ticket);
        Task DeleteAsync(int id);
    }
}
