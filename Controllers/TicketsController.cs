using CourseProjectItems.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseProjectItems.Controllers
{
	[Authorize]
	public class TicketsController : Controller
	{
		private readonly ITicketsRepository _ticketsRepository;

		public TicketsController(ITicketsRepository ticketsRepository)
		{
			_ticketsRepository = ticketsRepository;
		}

		public async Task<IActionResult> Index()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var tickets = await _ticketsRepository.GetTicketsByUserIdAsync(userId);

			if (tickets == null || !tickets.Any())
			{
				ViewBag.Message = "Тикеты пока не созданы";
			}

			return View("Index" , tickets);
		}
        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _ticketsRepository.GetByIdAsync(id);

            if (ticket == null)
            {
				return View("NotFound" , "Ticket was Not Found ");
            }

          
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ticket.UserId != userId)
            {
				return View("Forbidden", "Its not yours Tiket");
            }

            return View(ticket);
        }
    }
}
