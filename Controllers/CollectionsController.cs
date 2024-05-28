using Microsoft.AspNetCore.Mvc;
using CourseProjectItems.Interfaces;
using CourseProjectItems.Models;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using System.Text;
using CsvHelper;
using CourseProjectItems.ViewModels;
using Microsoft.EntityFrameworkCore;
using CourseProjectItems.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using CsvHelper.Configuration;


namespace CourseProjectItems.Controllers
{
	public class CollectionsController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IGenericRepository<Collection> _collectionRepository;

		public CollectionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IGenericRepository<Collection> collectionRepository)
		{
			_context = context;
			_userManager = userManager;
			_collectionRepository = collectionRepository;
		}

		[AllowAnonymous]
		public async Task<IActionResult> Index()
		{
            var collections = new List<Collection>();

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                collections = await _context.Collections.Where(c => c.AuthorID == user.Id).ToListAsync();
            }
            else
            {
                collections = await _context.Collections.ToListAsync(); 
            }

            var viewModel = collections.Select(c => new CollectionViewModel
			{
				Id = c.Id,
				Name = c.Name,
				Description = c.Description,
				Type = c.Type,
				Category = c.Category,
				AuthorId = c.AuthorID,
				AuthorUserName = c.AuthorUserName,
				IconClass = c.IconClass,
				Items = c.Items.Select(i => new ItemViewModel
				{
					Id = i.Id,
					Name = i.Name,
					CollectionId = i.CollectionId,
					CollectionName = c.Name,
					AuthorUserName = c.AuthorUserName,
					Comments = i.Comments.Select(comment => new CommentViewModel
					{
						Id = comment.Id,
						Message = comment.Message,
						UserName = comment.UserName,
						DateTime = comment.DateTime
					}).ToList(),
					Likes = i.Likes.Select(like => new LikeViewModel
					{
						Id = like.Id,
						UserId = like.UserId
					}).ToList()
				}).ToList()
			}).ToList();

			return View(viewModel);
		}

		[AllowAnonymous]
		public async Task<IActionResult> Details(int id)
		{
			var collection = await _collectionRepository.Find(c => c.Id == id)
				.Include(c => c.Items)
				.ThenInclude(i => i.Comments)
				.Include(c => c.Items)
				.ThenInclude(i => i.Likes)
				.FirstOrDefaultAsync();

			if (collection == null)
			{
				return View("NotFound", "The collection you are trying to view does not exist.");
			}

			var viewModel = new CollectionViewModel
			{
				Id = collection.Id,
				Name = collection.Name,
				Description = collection.Description,
				Type = collection.Type,
				Category = collection.Category,
				AuthorId = collection.AuthorID,
				IconClass = collection.IconClass,
				AuthorUserName = collection.AuthorUserName,
				Items = collection.Items.Select(i => new ItemViewModel
				{
					Id = i.Id,
					Name = i.Name,
					CollectionId = i.CollectionId,
					CollectionName = collection.Name,
					PhotoUrl = i.PhotoUrl,
					Tags = i.Tags,
					AdditionalFields = i.AdditionalFields,
					AuthorUserName = i.AuthorUserName,
					Comments = i.Comments.Select(comment => new CommentViewModel
					{
						Id = comment.Id,
						Message = comment.Message,
						UserName = comment.UserName,
						DateTime = comment.DateTime
					}).ToList(),
					Likes = i.Likes.Select(like => new LikeViewModel
					{
						Id = like.Id,
						UserId = like.UserId
					}).ToList()
				}).ToList()
			};

			return View(viewModel);
		}
		// GET: Collections/Create
		public IActionResult Create()
		{
			return View();
		}

		[Authorize(Roles = UserRoles.Admin + "," + UserRoles.User)]
		public IActionResult CreateItem(int collectionId)
		{
			return RedirectToAction("Create", "Items", new { collectionId });
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = UserRoles.Admin + "," + UserRoles.User)]
		public async Task<IActionResult> Create(CollectionViewModel viewModel)
		{
			var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
			var userName = User.Identity.Name ?? "Admin";
			if (user == null || user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.Now || user.IsBlocked)
            {
                return View("Unauthorized", "You do not have permission to create a collection.");
            }
            if (userId == null)
			{
				return View("Unauthorized", "You need to log in to create a collection.");
			}

			var collection = new Collection
			{
				Name = viewModel.Name,
				Description = viewModel.Description,
				AuthorID = userId,
				AuthorUserName = userName,
				Type = viewModel.Type,
				Category = viewModel.Category,
				PhotoUrl = string.Empty,
				IconClass = viewModel.IconClass
			
			};
			await _collectionRepository.Add(collection);
			return RedirectToAction(nameof(Index));
		}


		[Authorize(Roles = UserRoles.Admin + "," + UserRoles.User)]
		public async Task<IActionResult> Edit(int id)
		{
			var collection = await _collectionRepository.GetById(id);
			if (collection == null)
			{
				return View("NotFound", "The collection you are trying to edit does not exist.");
			}

			var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.Now || user.IsBlocked)
            {
                return View("Unauthorized", "You do not have permission to create a collection.");
            }
            if (collection.AuthorID != userId && !User.IsInRole(UserRoles.Admin))
			{
				return View("Forbidden", "Only the author or an admin can edit this collection.");
			}
			var viewModel = new CollectionViewModel
			{
				Id = collection.Id,
				Name = collection.Name,
				Description = collection.Description,
				AuthorId = collection.AuthorID,
				Type = collection.Type,
				Category = collection.Category,
				PhotoUrl = string.Empty,
				IconClass = collection.IconClass,
				AuthorUserName = collection.AuthorUserName
			};

			return View(viewModel);
		}

		// POST: Collections/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.User)]
        public async Task<IActionResult> Edit(int id, CollectionViewModel viewModel)
		{
			if (id != viewModel.Id)
			{
				return View("NotFound", "The collection you are trying to edit does not exist.");
			}

			var collection = await _collectionRepository.GetById(id);
			if (collection == null)
			{
				return View("NotFound", "The collection you are trying to edit does not exist.");
			}

			var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.Now || user.IsBlocked)
            {
                return View("Unauthorized", "You do not have permission to create a collection.");
            }
            if (collection.AuthorID != userId && !User.IsInRole(UserRoles.Admin))
			{
				return View("Forbidden", "Only the author or an admin can edit this collection.");
			}
			

			collection.Id = viewModel.Id;
            collection.Name = viewModel.Name;
            collection.Description = viewModel.Description;
			collection.AuthorID = viewModel.AuthorId;
            collection.Type = viewModel.Type;
            collection.Category = viewModel.Category;
			collection.PhotoUrl = String.Empty;
            collection.IconClass = viewModel.IconClass;
			collection.AuthorUserName = user.UserName ?? "Admin";

            await _collectionRepository.Update(collection);
			return RedirectToAction(nameof(Index));
		}

		[Authorize(Roles = UserRoles.Admin + "," + UserRoles.User)]
		public async Task<IActionResult> Delete(int id)
		{
			var collection = await _collectionRepository.GetById(id);
			if (collection == null)
			{
				return View("NotFound", "The collection you are trying to delete does not exist.");
			}

			var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.Now || user.IsBlocked)
            {
                return View("Unauthorized", "You do not have permission to create a collection.");
            }
            if (collection.AuthorID != userId && !User.IsInRole(UserRoles.Admin))
			{
				return View("Forbidden", "Only the author or an admin can delete this collection.");
			}
			var viewModel = new CollectionViewModel
			{
				Id = collection.Id,
				Name = collection.Name,
				Description = collection.Description
			};

			return View(viewModel);
		}

		[Authorize(Roles = UserRoles.Admin + "," + UserRoles.User)]
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{

			var collection = await _context.Collections
				.Include(c => c.Items)
				.FirstOrDefaultAsync(c => c.Id == id);

			if (collection == null)
			{
				return View("NotFound", "The collection you are trying to delete does not exist.");
			}

			var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.Now || user.IsBlocked)
            {
                return View("Unauthorized", "You do not have permission to create a collection.");
            }
            if (collection.AuthorID != userId && !User.IsInRole(UserRoles.Admin))
			{
				return View("Forbidden", "Only the author or an admin can delete this collection.");
			}


			var items = await _context.Items.Where(i => i.CollectionId == id).ToListAsync();
			foreach (var item in items)
			{
				var comments = await _context.Comments.Where(c => c.ItemId == item.Id).ToListAsync();
				_context.Comments.RemoveRange(comments);
				_context.Items.Remove(item);
			}

			_context.Collections.Remove(collection);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> ExportToCsv(int id)
		{
			var collection = await _context.Collections
				.Include(c => c.Items)
				.FirstOrDefaultAsync(c => c.Id == id);

			if (collection == null)
			{
				return NotFound();
			}

			var items = collection.Items;

			using (var memoryStream = new MemoryStream())
			using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
			using (var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = "," }))
			{
				// Настраиваем заголовки
				csvWriter.WriteField("ID");
				csvWriter.WriteField("Name");
				csvWriter.WriteField("Description");
				csvWriter.WriteField("Tags");
				csvWriter.WriteField("Additional Fields");
				csvWriter.WriteField("Photo URL");
				csvWriter.WriteField("Created By");
				csvWriter.NextRecord();

				// Записываем данные
				foreach (var item in items)
				{
					csvWriter.WriteField(item.Id);
					csvWriter.WriteField(item.Name);
					csvWriter.WriteField(item.Description);
					csvWriter.WriteField(item.Tags);
					csvWriter.WriteField(item.AdditionalFields);
					csvWriter.WriteField(item.PhotoUrl);
					csvWriter.WriteField(item.AuthorUserName == null ? "Admin" : item.AuthorUserName);
					csvWriter.NextRecord();
				}

				writer.Flush();
				var fileName = $"{collection.Name}_Items.csv";
				return File(memoryStream.ToArray(), "text/csv", fileName);
			}
		}


	}
}
