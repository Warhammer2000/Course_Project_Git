using Microsoft.AspNetCore.Mvc;
using CourseProjectItems.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CourseProjectItems.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PhotosController : ControllerBase
	{
		private readonly IPhotoService _photoService;

		public PhotosController(IPhotoService photoService)
		{
			_photoService = photoService;
		}

		[HttpPost("upload")]
		public async Task<IActionResult> UploadPhoto(IFormFile file)
		{
			var result = await _photoService.AddPhotoAsync(file);

			if (result.Error != null)
				return BadRequest(result.Error.Message);

			return Ok(new { result.Url });
		}

		[HttpDelete("delete")]
		public async Task<IActionResult> DeletePhoto(string publicId)
		{
			var result = await _photoService.DeletePhotoAsync(publicId);

			if (result.Error != null)
				return BadRequest(result.Error.Message);

			return Ok(result.Result);
		}
	}
}
