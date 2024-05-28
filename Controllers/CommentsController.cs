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


namespace CourseProjectItems.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ApplicationDbContext _context;
        public CommentsController(ICommentRepository commentRepository, ApplicationDbContext context)
        {
            _commentRepository = commentRepository;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(Comment comment)
        {
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
            var comment = await _commentRepository.GetById(id);
            if (comment != null)
            {
                await _commentRepository.Delete(id);
                return RedirectToAction("Details", "Items", new { id = comment.ItemId });
            }
            return NotFound();
        }
    }
}
