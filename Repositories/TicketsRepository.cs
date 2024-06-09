using CourseProjectItems.Data;
using CourseProjectItems.Interfaces;
using CourseProjectItems.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProjectItems.Repositories
{
	public class TicketsRepository : ITicketsRepository
	{
        private readonly ApplicationDbContext _context;

        public TicketsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByUserIdAsync(string userId)
        {
            return await _context.Tickets
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task AddAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task<Ticket> GetByIdAsync(int id)
        {
            return await _context.Tickets.FindAsync(id);
        }

        public async Task UpdateAsync(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }
    }
}
