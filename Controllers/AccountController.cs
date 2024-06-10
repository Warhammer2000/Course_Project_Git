using CourseProjectItems.Data;
using CourseProjectItems.Interfaces;
using CourseProjectItems.Models;
using CourseProjectItems.Models.Enums;
using CourseProjectItems.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseProjectItems.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailSender = emailSender;
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (!ModelState.IsValid) return View(loginViewModel);

            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);

            if (user != null)
            {
                if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow)
                {
                    ModelState.AddModelError(string.Empty, "Account is locked");
                    return View(loginViewModel);
                }

                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false);
                    if (result.Succeeded)
                    {
                        JiraController.currentUserEmail = loginViewModel.Email;
                        return LocalRedirect(returnUrl);
                    }
                    //if (result.IsLockedOut)
                    //{
                    //    ModelState.AddModelError(string.Empty, "Account is locked");
                    //    return View(loginViewModel);
                    //}
                }

                //ModelState.AddModelError(string.Empty, "Wrong credentials. Please try again");
                //return View(loginViewModel);
            }
            ModelState.AddModelError(string.Empty, "Wrong credentials. Please try again");
            return View(loginViewModel);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            var user = await _userManager.FindByEmailAsync(registerViewModel.Email);
            if (user != null)
            {
                ModelState.AddModelError(string.Empty, "This email address is already in use");
                Console.WriteLine("This email address is already in use");
                return View(registerViewModel);
            }

            var newUser = new ApplicationUser()
            {
                LockoutEnabled = false,
                Email = registerViewModel.Email,
                UserName = registerViewModel.UserName,
                ApiToken = String.Empty,
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            if (newUserResponse.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                var callbackUrl = Url.Action(
                    "ConfirmEmail", "Account",
                    new { userId = newUser.Id, code = code },
                    protocol: HttpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(registerViewModel.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.");

                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                await _signInManager.SignInAsync(newUser, isPersistent: false);
                return RedirectToAction("RegisterConfirmation", new { email = registerViewModel.Email });
            }

            foreach (var error in newUserResponse.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                Console.WriteLine("error " + error.Description);
            }
            return View(registerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> UserPage(CollectionType? type, string id, string name = "default", int page = 1, SortState sortOrder = SortState.NameAsc)
        {
            string userId;
            if (string.IsNullOrEmpty(id))
            {
                userId = _userManager.GetUserId(User);
            }
            else
            {
                if (!name.Equals("default"))
                {
                    userId = _userManager.GetUserId(User);
                }
                else userId = id;
            }

            name = _context.Users.Find(userId).UserName;
            string emailAdress = _context.Users.Find(userId).Email;

            IQueryable<Collection> collections = _context.Collections.Where(m => m.AuthorID == userId);

            var viewModelOptions = new ViewModelOptions<Collection>()
            {
                Collections = collections
            }.GetSortedAndFilteredCollection(type, page, sortOrder);
            UserPageViewModel viewModel = new UserPageViewModel
            {
                UserName = name,
                Email = emailAdress,
                PageViewModel = new PageViewModel(viewModelOptions.Result.Count(), page, viewModelOptions.PageSizeCollection),
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(viewModelOptions.Result, type, nameof(type)),
                Collections = viewModelOptions.Result
            };
            return View(viewModel);
        }
        [HttpGet]
        public IActionResult RegisterConfirmation(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            ViewBag.StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            return View();
        }

    }
}
