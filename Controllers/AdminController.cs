using CourseProjectItems.Data;
using CourseProjectItems.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.EntityFrameworkCore;

namespace CourseProjectItems.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHtmlLocalizer<AdminController> _localizer;
        private readonly ILogger<AdminController> _logger;
        public AdminController(UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager, IHtmlLocalizer<AdminController> localizer, ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _localizer = localizer;
            _logger = logger;

            _logger.LogInformation("AdminController initialized with _localizer: {LocalizerExists}", _localizer != null);
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = users.Select(user => new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsLockedOut = user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow,
                IsAdmin = _userManager.IsInRoleAsync(user, "Admin").Result,
                IsBlocked = user.IsBlocked
            }).ToList();

            return View(userViewModels);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (userId == null) return View("NotFound", "User not found.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                TempData["SuccessMessage"] = _localizer["User has been deleted."].Value;
            }
            else
            {
                TempData["WarningMessage"] = _localizer["User not found."].Value;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> BlockUser(string userId)
        {
            if (userId == null) return View("NotFound", "User not found.");
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) TempData["WarningMessage"] = _localizer["User not found."].Value;
            else
            {
                user.IsBlocked = true;
                await _userManager.UpdateAsync(user);
                TempData["WarningMessage"] = _localizer["User {0} has been blocked.", user.UserName].Value;
            }
          
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> LockUser(string userId)
        {
            if (userId == null) return View("NotFound", "User not found.");
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.LockoutEnabled = true;
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
                await _userManager.UpdateAsync(user);
                TempData["WarningMessage"] = _localizer["User {0} has been locked.", user.UserName].Value;
            }
            else TempData["WarningMessage"] = _localizer["User not found."].Value;
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> UnblockUser(string userId)
        {
            if (userId == null) return View("NotFound", "User not found.");
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) TempData["WarningMessage"] = _localizer["User not found."].Value;
            else
            {
                user.IsBlocked = false;
                await _userManager.UpdateAsync(user);
                TempData["SuccessMessage"] = _localizer["User {0} has been unblocked.", user.UserName].Value;
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> UnlockUser(string userId)
        {
            if (userId == null) return View("NotFound", "User not found.");
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                user.LockoutEnabled = false;
                await _userManager.UpdateAsync(user);
                TempData["SuccessMessage"] = _localizer["User {0} has been unlocked.", user.UserName].Value;
            }
            else TempData["WarningMessage"] = _localizer["User not found."].Value;
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> AddAdmin(string userId)
        {
            if (userId == null)
            {
                return View("NotFound", "User not found.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && !await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                TempData["SuccessMessage"] = _localizer["User has been added to Admins."].Value;
            }
            else
            {
                TempData["WarningMessage"] = _localizer["User not found or already an Admin."].Value;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAdmin(string userId)
        {
            if (userId == null)
            {
                return View("NotFound", "User not found.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
                TempData["SuccessMessage"] = _localizer["User has been removed from Admins."].Value;
            }
            else
            {
                TempData["WarningMessage"] = _localizer["User not found or not an Admin."].Value;
            }

            return RedirectToAction("Index");
        }
    }
}
