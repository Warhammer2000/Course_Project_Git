using Microsoft.AspNetCore.Mvc;
using CourseProjectItems.Interfaces;
using CourseProjectItems.Models;
using CourseProjectItems.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CourseProjectItems.Data;
using System.Security.Claims;
using CourseProjectItems.Repositories;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace CourseProjectItems.Controllers
{
	[AllowAnonymous]
	public class ItemsController : Controller
	{
		private readonly IGenericRepository<Item> _itemRepository;
		private readonly IGenericRepository<Collection> _collectionRepository;
		private readonly IPhotoService _photoService;
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public ItemsController(IGenericRepository<Item> itemRepository, 
			IGenericRepository<Collection> collectionRepository, 
			IPhotoService photoService, ApplicationDbContext context, 
			UserManager<ApplicationUser> userManager)
		{
			_itemRepository = itemRepository;
			_collectionRepository = collectionRepository;
			_photoService = photoService;
			_context = context;
			_userManager = userManager;
		}

		[AllowAnonymous]
		public async Task<IActionResult> Index()
		{
			var items = await _itemRepository.GetAll();
			var viewModel = items.Select(i => new ItemViewModel
			{
				Id = i.Id,
				Name = i.Name,
				CollectionId = i.CollectionId,
				CollectionName = i.CollectionName,
				Tags = i.Tags,
				AdditionalFields = i.AdditionalFields,
				PhotoUrl = i.PhotoUrl,
				AuthorUserName = i.AuthorUserName,
				Description = i.Description ?? "No Description",
				Comments = i.Comments.Select(c => new CommentViewModel
				{
					Id = c.Id,
					Message = c.Message,
					UserName = c.UserName,
					DateTime = c.DateTime
				}).ToList(),
				Likes = i.Likes.Select(l => new LikeViewModel
				{
					Id = l.Id,
					UserId = l.UserId
				}).ToList()
			}).ToList();

			return View(viewModel);
		}

		[AllowAnonymous]
		public async Task<IActionResult> Details(int id)
		{
			var query = _itemRepository.Find(i => i.Id == id)
				.Include(i => i.Collection)
				.Include(i => i.Comments)
				.Include(i => i.Likes);

			var items = await query.ToListAsync();
			foreach (var itm in items)
			{
				Console.WriteLine($"Item ID: {itm.Id}, Collection: {itm.Collection.Name}, Comments: {itm.Comments.Count}, Likes: {itm.Likes.Count}");
			}

			var item = items.FirstOrDefault();
			if (item == null)
			{
                return View("NotFound", "The item you are trying to comment on does not exist.");
            }

			var viewModel = new ItemViewModel
			{
				Id = item.Id,
				Name = item.Name ?? "No Name",
				CollectionId = item.CollectionId,
				CollectionName = item.CollectionName?? "No Collection",
				PhotoUrl = item.PhotoUrl ?? string.Empty,
				Tags = item.Tags ?? string.Empty,
				AdditionalFields = item.AdditionalFields ?? "No Additional Fields",
				AuthorUserName= item.AuthorUserName ?? "Admin",
                Description = item.Description,
                Comments = item.Comments?.Select(c => new CommentViewModel
				{
					Id = c.Id,
					Message = c.Message ?? "No Message",
					UserName = c.UserName ?? "Anonymous",
					DateTime = c.DateTime
				}).ToList() ?? new List<CommentViewModel>(),
				Likes = item.Likes?.Select(l => new LikeViewModel
				{
					Id = l.Id,
					UserId = l.UserId ?? "No User"
				}).ToList() ?? new List<LikeViewModel>()
			};

			return View(viewModel);
		}

		[Authorize(Roles = UserRoles.Admin + "," + UserRoles.User)]
		public async Task<IActionResult> Create(int collectionId)
		{
			if (collectionId == 0)
			{
				return View("BadRequest","Collection ID cannot be zero.");
			}


			var collection = await _collectionRepository.GetById(collectionId);
			if (collection == null)
			{
                return View("NotFound", "The item you are trying to comment on does not exist.");
            }

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.Now || user.IsBlocked)
            {
                return View("Unauthorized", "You do not have permission to create an item.");
            }

            var viewModel = new ItemViewModel
			{
				CollectionId = collectionId,
				CollectionName = collection.Name,
				AuthorUserName = collection.AuthorUserName,
				AuthorId = User.FindFirstValue(ClaimTypes.NameIdentifier),
				Comments = new List<CommentViewModel> { new CommentViewModel { UserName = user.UserName, UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) } }
			};

			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.User)]
        public async Task<IActionResult> Create(ItemViewModel viewModel)
		{

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null || user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.Now || user.IsBlocked)
            {
                return View("Unauthorized", "You do not have permission to edit this item.");
            }

            var item = new Item
			{
				Name = viewModel.Name,
				Description = viewModel.Description,
				CollectionId = viewModel.CollectionId,
				CollectionName = viewModel.CollectionName,
				AuthorId = viewModel.AuthorId,
				Tags = viewModel.Tags,
				AuthorUserName = user.UserName ?? "Admin",
				AdditionalFields = viewModel.AdditionalFields
			};

			if (viewModel.Photo != null)
			{
				var uploadResult = await _photoService.AddPhotoAsync(viewModel.Photo);
				if (uploadResult != null)
				{
					item.PhotoUrl = uploadResult.Url.ToString();
					viewModel.PhotoUrl = uploadResult.Url.ToString();
				}
			}
			await _itemRepository.Add(item);

			if (!string.IsNullOrWhiteSpace(viewModel.Comments[0].Message))
			{
				var comment = new Comment
				{
					ItemId = item.Id,
					Message = viewModel.Comments[0].Message,
					UserName = user.UserName ?? "Admin",
					UserId = viewModel.Comments[0].UserId,
					DateTime = DateTime.Now
				};
				await _context.Comments.AddAsync(comment);
				await _context.SaveChangesAsync();
			}
			await _context.SaveChangesAsync();
			return RedirectToAction("Details", "Collections", new { id = viewModel.CollectionId });
		}

		[Authorize(Roles = UserRoles.Admin + "," + UserRoles.User)]
		public async Task<IActionResult> Edit(int id)
		{
			var item = await _itemRepository.Find(i => i.Id == id)
				.Include(i => i.Collection)
				.FirstOrDefaultAsync();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

			if (item == null)
			{
				return View("NotFound", "The item you are trying to comment on does not exist.");
			}
			if (user == null || user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.Now || user.IsBlocked)
            {
                return View("Unauthorized", "You do not have permission to edit this item.");
            }
			if (item.AuthorId != userId && !User.IsInRole(UserRoles.Admin))
			{
				return View("Forbidden", "Only the author or an admin can delete this collection.");
			}
			

			var viewModel = new ItemViewModel
			{
				Id = item.Id,
				Name = item.Name,
				Description = item.Description,
				CollectionId = item.CollectionId,
				CollectionName = item.CollectionName,
				PhotoUrl = item.PhotoUrl,
				Tags = item.Tags,
				AdditionalFields = item.AdditionalFields,
				AuthorUserName = item.AuthorUserName
			};

			return View(viewModel);
		}

		[Authorize(Roles = UserRoles.Admin + "," + UserRoles.User)]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, ItemViewModel viewModel)
		{
			if (id != viewModel.Id)
			{
                return View("NotFound", "The item you are trying to comment on does not exist.");
            }

			var item = await _itemRepository.GetById(id);
			if (item == null)
			{
                return View("NotFound", "The item you are trying to comment on does not exist.");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
			if (item.AuthorId != userId && !User.IsInRole(UserRoles.Admin))
			{
				return View("Forbidden", "Only the author or an admin can delete this collection.");
			}
			if (user == null || user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.Now || user.IsBlocked)
            {
                return View("Unauthorized", "You do not have permission to edit this item.");
            }

            item.Name = viewModel.Name;
			item.Tags = viewModel.Tags;

			if (viewModel.CollectionId == 0)
			{
				viewModel.CollectionId = item.CollectionId;
			}
			item.CollectionId = viewModel.CollectionId;
			if(viewModel.CollectionName == null)
			{
				viewModel.CollectionName = item.CollectionName;
			}
			else item.CollectionName = viewModel.CollectionName;

			item.AdditionalFields = viewModel.AdditionalFields;

			if (viewModel.Photo != null)
			{
				var uploadResult = await _photoService.AddPhotoAsync(viewModel.Photo);
				if (uploadResult != null)
				{
					item.PhotoUrl = uploadResult.Url.ToString();
					viewModel.PhotoUrl = uploadResult.Url.ToString();
				}
			}

			Console.WriteLine(" item photoUrl " + item.PhotoUrl + " ViewModel.Photo Url " + viewModel.PhotoUrl);
			await _context.SaveChangesAsync();
			await _itemRepository.Update(item);
			return RedirectToAction("Details", "Collections", new { id = viewModel.CollectionId });

		}

		[Authorize(Roles = UserRoles.Admin + "," + UserRoles.User)]
		public async Task<IActionResult> Delete(int id)
		{
			var item = await _itemRepository.GetById(id);
			if (item == null)
			{
                return View("NotFound", "The item you are trying to comment on does not exist.");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
			if (item.AuthorId != userId && !User.IsInRole(UserRoles.Admin))
			{
				return View("Forbidden", "Only the author or an admin can delete this collection.");
			}
			if (user == null || user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.Now || user.IsBlocked)
            {
                return View("Unauthorized", "You do not have permission to edit this item.");
            }
            var viewModel = new ItemViewModel
			{
				Id = item.Id,
				Name = item.Name,
				CollectionId = item.CollectionId
			};

			return View(viewModel);
		}

		[Authorize(Roles = UserRoles.Admin + "," + UserRoles.User)]
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var item = await _itemRepository.GetById(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null || user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.Now || user.IsBlocked)
            {
                return View("Unauthorized", "You do not have permission to edit this item.");
            }
			if (item.AuthorId != userId && !User.IsInRole(UserRoles.Admin))
			{
				return View("Forbidden", "Only the author or an admin can delete this collection.");
			}
			if (item != null)
			{
				var comments = _context.Comments.Where(c => c.ItemId == id);
				_context.Comments.RemoveRange(comments);

				var likes = _context.Likes.Where(l => l.ItemId == id);
				_context.Likes.RemoveRange(likes);

				await _context.SaveChangesAsync();

				await _itemRepository.Delete(id);
			}
			return RedirectToAction(nameof(Index));
		}
        [HttpPost]	
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int itemId, CommentViewModel commentViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Details", new { id = itemId });
            }

            var item = await _itemRepository.GetById(itemId);
            if (item == null)
            {
                return View("NotFound", "The item you are trying to comment on does not exist.");
            }

            var userId = _userManager.GetUserId(User);
            var comment = new Comment
            {
                ItemId = itemId,
                Message = commentViewModel.Message,
                UserName = item.AuthorUserName,
                UserId = userId,
                DateTime = DateTime.Now
            };

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = itemId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.User)]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return View("NotFound", "The comment you are trying to delete does not exist.");
            }

            var item = await _itemRepository.GetById(comment.ItemId);
            if (item == null)
            {
                return View("NotFound", "The item related to the comment does not exist.");
            }

            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            if (item.AuthorId != userId && !User.IsInRole(UserRoles.Admin) || user.IsBlocked)
            {
                return View("Unauthorized", "You do not have permission to delete this comment.");
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = item.Id });
        }
    }
}
