using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CourseProjectItems.Interfaces;
using CourseProjectItems.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using CourseProjectItems.Data;
using Microsoft.AspNetCore.Identity;


namespace CourseProjectItems.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CommentsController(ICommentRepository commentRepository, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _commentRepository = commentRepository;
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            var isAuthenticatedAndEmailConfirmed = await IsAuthenticatedAndEmailConfirmed(_userManager);
            if (!isAuthenticatedAndEmailConfirmed)
            {
                return View("Unauthorized", "You do not have permission to comment. Please confirm your e-mail.");
            }

            comment.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            comment.UserName = User.Identity.Name;
            comment.DateTime = DateTime.Now;

            var item = await _context.Items.FindAsync(comment.ItemId);
            if (item != null)
            {
                comment.Item = item;
            }

            await _commentRepository.Add(comment);
            return RedirectToAction("Details", "Items", new { id = comment.ItemId });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var isAuthenticatedAndEmailConfirmed = await IsAuthenticatedAndEmailConfirmed(_userManager);
            if (!isAuthenticatedAndEmailConfirmed)
            {
                return View("Unauthorized", "You do not have permission to comment. Please confirm your e-mail.");
            }

            var comment = await _commentRepository.GetById(id);
            if (comment != null)
            {
                await _commentRepository.Delete(id);
                return RedirectToAction("Details", "Items", new { id = comment.ItemId });
            }
            return NotFound();
        }
        public async Task<bool> IsAuthenticatedAndEmailConfirmed(UserManager<ApplicationUser> userManager)
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                return await userManager.IsEmailConfirmedAsync(user);
            }
            return false;
        }
    }
}
