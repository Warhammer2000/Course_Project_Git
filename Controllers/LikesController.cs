using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CourseProjectItems.Interfaces;
using CourseProjectItems.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace CourseProjectItems.Controllers
{
	[Authorize]
	public class LikesController : Controller
	{
        private readonly ILikeRepository _likeRepository;

        public LikesController(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Like(int itemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!await _likeRepository.IsLikedByUser(itemId, userId))
            {
                var like = new Like { ItemId = itemId, UserId = userId };
                await _likeRepository.Add(like);
            }
            return RedirectToAction("Details", "Items", new { id = itemId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unlike(int itemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var like = await _likeRepository.Find(l => l.ItemId == itemId && l.UserId == userId).FirstOrDefaultAsync();
            if (like != null)
            {
                await _likeRepository.Delete(like.Id);
            }
            return RedirectToAction("Details", "Items", new { id = itemId });
        }
    }
}
