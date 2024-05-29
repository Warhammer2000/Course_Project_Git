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
            if (file == null)
            {
                return BadRequest("No file uploaded.");
            }

            var validExtensions = new[] { ".png", ".jpeg", ".jpg" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!validExtensions.Contains(extension))
            {
                return BadRequest("Invalid file type. Only .png, .jpeg, and .jpg files are allowed.");
            }

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            return Ok(new { result.Url });
        }


        [HttpDelete("delete")]
        public async Task<IActionResult> DeletePhoto(string publicId)
        {
            var result = await _photoService.DeletePhotoAsync(publicId);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            return Ok(result.Result);
        }
    }
}
