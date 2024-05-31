using CourseProjectItems.Data;
using CourseProjectItems.Models;
using CourseProjectItems.ViewModels;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using CourseProjectItems.Interfaces;

namespace CourseProjectItems.Controllers
{
	public class HomeController : Controller
	{
		private readonly IGenericRepository<Collection> _collectionRepository;
		private readonly ApplicationDbContext _context;
		public HomeController(ApplicationDbContext context, IGenericRepository<Collection> collectionRepository)
		{
			_context = context;
			_collectionRepository = collectionRepository;
		}
        public async Task<IActionResult> Index(string searchString)
        {
            var itemsQuery = _context.Items.AsQueryable();
            var collectionsQuery = _context.Collections.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                string formattedSearchString = "\"" + searchString + "\"";
                itemsQuery = itemsQuery.Where(i => EF.Functions.Contains(i.Name, formattedSearchString) || EF.Functions.Contains(i.Description, formattedSearchString));
                collectionsQuery = collectionsQuery.Where(c => EF.Functions.Contains(c.Name, formattedSearchString) || EF.Functions.Contains(c.Description, formattedSearchString));
            }

            var searchViewModel = new SearchViewModel
            {
                Items = await itemsQuery.ToListAsync(),
                Collections = await collectionsQuery.ToListAsync()
            };

            var collections = await _collectionRepository.GetAll();
            var viewModel = collections.Select(c => new CollectionViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Category = c.Category,
                AuthorId = c.AuthorID,
                Items = c.Items.Select(i => new ItemViewModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    CollectionId = i.CollectionId,
                    CollectionName = c.Name,
                    PhotoUrl = i.PhotoUrl,
                    Tags = i.Tags,
                    AdditionalFields = i.AdditionalFields,
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

            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;
            var languages = new List<SelectListItem>
            {
                  new SelectListItem { Value = "en-US", Text = "English", Selected = currentCulture == "en-US" },
                  new SelectListItem { Value = "ru-RU", Text = "Russian", Selected = currentCulture == "ru-RU" }
            };

            ViewBag.LanguageOptions = GenerateLanguageOptions(languages);

            return View(new HomeIndexViewModel { SearchResults = searchViewModel, Collections = viewModel });
        }

        private string GenerateLanguageOptions(List<SelectListItem> languages)
        {
            var sb = new StringBuilder();
            foreach (var language in languages)
            {
                sb.Append($"<option value=\"{language.Value}\"{(language.Selected ? " selected" : "")}>{language.Text}</option>");
            }
            return sb.ToString();
        }


        [HttpPost]
		public IActionResult SetLanguage(string culture, string returnUrl)
		{
			Response.Cookies.Append(
				CookieRequestCultureProvider.DefaultCookieName,
				CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
				new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
			);

			return LocalRedirect(returnUrl);
		}
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}