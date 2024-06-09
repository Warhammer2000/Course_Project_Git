using CourseProjectItems.Data;
using CourseProjectItems.Interfaces;
using CourseProjectItems.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CourseProjectItems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionsApiController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public CollectionsApiController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCollections()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var collections = await _userManager.FindByIdAsync(userId);
            return Ok(collections);
        }

        [HttpGet("GenerateToken")]
        [Authorize]
        public IActionResult ShowGenerateTokenForm()
        {
            return View("ShowGenerateTokenForm");
        }


        [HttpPost("GenerateToken")]
        [Authorize]
        public async Task<IActionResult> GenerateToken(string empty = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Генерация исходного JWT Token
            var originalJwtToken = CreateJWTToken();

            // Генерация API Token из JWT Token
            var apiToken = GenerateApiToken(userId, originalJwtToken);

            var user = await _userManager.FindByIdAsync(userId);
            user.ApiToken = apiToken;
            await _userManager.UpdateAsync(user);

            await SaveTokenForUser(userId, apiToken);

            // Возвращаем как оригинальный JWT Token, так и API Token
            var tokens = new Tuple<string, string>(originalJwtToken, apiToken);
            return View("TokenGenerated", tokens);
        }

        private string GenerateApiToken(string userId, string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtToken); 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userId) }),
                Expires = DateTime.UtcNow.AddMonths(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private string CreateJWTToken()
        {
            var key = new byte[32]; 
            RandomNumberGenerator.Fill(key);
            return Convert.ToBase64String( key);
        }
        private Task SaveTokenForUser(string userId, string token)
        {
            // Реализуйте сохранение токена для пользователя, например, в базе данных
            // Например:
            // await _userRepository.SaveApiTokenAsync(userId, token);
            return Task.CompletedTask;
        }
    }
}
